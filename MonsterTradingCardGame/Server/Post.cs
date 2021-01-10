using MonsterTradingCardGame.Battle;
using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.DB;
using MonsterTradingCardGame.User;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class Post : REST
    {
        public Post( string content, Dictionary<string, string> hv )
        {
            setREST(content, hv);
        }

        //Message handling functions
        public void HandlePostUsersMessage()
        {
            CUser newUser = ConvertJsonUserContent();

            Connector con = new Connector();

            //If user already exists in DB exit methode
            if (CompareDbEntries(con.con, "SELECT username FROM users", newUser.Username) )
            {
                RespCode = Response.conflictCode;
                Resp = $"User {newUser.Username} already exists!";
                Console.WriteLine(Resp);
                return;
            }
            
            newUser.AddToDb(con.con);

            RespCode = Response.createCode;
            Resp = $"User {newUser.Username} successfully registered!";
            return;
        }

        public void HandlePostPackagesMessage()
        {
            if( Authorization.Length < 2 || Authorization[1] != "admin" )
            {
                CreateResponse(Response.forbiddenCode, "ERROR: Only admin is allowed to create packages");
                return;
            }

            Connector con = new Connector();

            CUser adminUser = new CUser(Authorization[1]);
            if (!adminUser.CheckLoggedIn(con.con))
            {
                CreateResponse(Response.unathorizedCode, "ERROR: Admin is not logged in");
                return;
            }

            List<CCard> cards = ConvertJsonCardsContent();
            if(cards.Count == 0)
            {
                CreateResponse(Response.notFoundCode, "No cards found for inserting!");
                return;
            }

            int package_id = cards[0].GetNextPackageId( con.con );
            foreach( CCard tmp_card in cards )
            {
                tmp_card.FinishCardDetails( con.con, package_id );
                tmp_card.AddToDb(con.con);
            }

            CreateResponse( Response.okCode, "Admin is logged in" );
            return;
        }

        internal void HandlePostBattlesMessage()
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User a MTCG user to continue!");
                return;
            }

            Connector con = new Connector();

            CUser fightUser = new CUser(Authorization[1]);
            if (!fightUser.CheckLoggedIn(con.con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {fightUser.Username} is not logged in");
                return;
            }

            CBattle battle = new CBattle(fightUser.Username);
            if (!battle.FindOpponent())
            {
                if(!battle.SetFighterAvailable())
                {
                    CreateResponse(Response.internalServerErrorCode, $"ERROR: Setting {fightUser.Username} to active was not working");
                    return;
                }
                else
                {
                    CreateResponse(Response.okCode, $"{fightUser.Username} was registered as active fighter!");
                    return;
                }
            }

            if(!battle.PrepareFighters())
            {
                CreateResponse(Response.internalServerErrorCode, $"ERROR: Preparation of fighters is not working correctly!");
                return;
            }

            if(!battle.PrepareEnvironment())
            {
                CreateResponse(Response.internalServerErrorCode, $"ERROR: Preparation of environment is not working correctly!");
                return;
            }

            string log = battle.Start();
            CreateResponse(Response.okCode, log);

            return;
        }

        internal void HandlePostTradingsMessage()
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User a MTCG user to continue!");
                return;
            }

            Connector con = new Connector();

            CUser activeUser = new CUser(Authorization[1]);
            if (!activeUser.CheckLoggedIn(con.con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {activeUser.Username} is not logged in");
                return;
            }

            CTradings newTrade = ConvertJsonTradingContent();

            string res = "\n";
            res += newTrade.Post(activeUser.Username);
            res += newTrade.Print();

            CreateResponse(Response.okCode, res);
            return;
        }

        internal void HandlePostTradingsIdMessage(string trade_id)
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User a MTCG user to continue!");
                return;
            }

            Connector con = new Connector();

            CUser activeUser = new CUser(Authorization[1]);
            if (!activeUser.CheckLoggedIn(con.con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {activeUser.Username} is not logged in");
                return;
            }

            CTradings trade = new CTradings(trade_id);
            CCard trade_card = new CCard(ConvertJsonStringContent());
            trade_card.CompleteCardInformation(con.con);

            string tradeOwner = trade.GetOwner();
            if( tradeOwner == activeUser.Username )
            {
                CreateResponse(Response.forbiddenCode, "ERROR: You can not make your own trade");
                return;
            }

            if( !trade_card.username.Equals(activeUser.Username))
            {
                CreateResponse(Response.forbiddenCode, "ERROR: You need to trade a card which belongs to you");
                return;
            }

            if(trade_card.CheckInDeck(con))
            {
                CreateResponse(Response.forbiddenCode, "ERROR: You card is currently used in your deck");
                return;
            }

            if(!trade.Trade(trade_card, tradeOwner))
            {
                CreateResponse(Response.forbiddenCode, "ERROR: Your card does not fit the requirements");
                return;
            }

            CreateResponse(Response.okCode, "Trade completed!");
        }

        public void HandlePostTransactionsPackagesMessage()
        {
            Connector con = new Connector();

            CUser buyUser = new CUser(Authorization[1]);
            if (!buyUser.CheckLoggedIn(con.con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {buyUser.Username} is not logged in");
                return;
            }

            if (!buyUser.CheckCoinsForPurchase(con.con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {buyUser.Username} do not have enough coins");
                return;
            }

            if (!buyUser.PurchasePackage(con.con))
            {
                CreateResponse(Response.conflictCode, $"ERROR: No package available at the moment");
                return;
            }

            CreateResponse(Response.okCode, $"Package was purchased successfully");
        }

        public void HandlePostSessionsMessage()
        {
            CUser loginUser = ConvertJsonUserContent();

            Connector con = new Connector();

            //If User is not found in DB check, exit methode
            if (!CompareDbEntries(con.con, "SELECT username FROM users", loginUser.Username))
            {
                RespCode = Response.conflictCode;
                Resp = $"User {loginUser.Username} not found!";
                Console.WriteLine(Resp);
                return;
            }

            if ( loginUser.setActive(con.con) == 1 )
            {
                RespCode = Response.okCode;
                Resp = $"User {loginUser.Username} logged in successfully!";
            }
            else
            {
                RespCode = Response.conflictCode;
                Resp = "Username and/or Password is incorrect!";
            }
            Console.WriteLine(Resp);
            return;
        }
    }
}
