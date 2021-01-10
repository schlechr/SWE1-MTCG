using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.DB;
using MonsterTradingCardGame.User;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class Put : REST
    {
        public Put(string content, Dictionary<string,string> hv)
        {
            setREST(content, hv);
        }

        internal void HandlePutDeckMessage()
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

            CDeck newDeck = new CDeck(con.con, ConvertJsonDeckContent());
            if( newDeck.cards.Count != 4 )
            {
                CreateResponse(Response.conflictCode, $"ERROR: There must be 4 cards declared for the deck!");
                return;
            }

            if( newDeck.GetCardsOwner() != activeUser.Username )
            {
                CreateResponse(Response.forbiddenCode, $"ERROR: One or more cards do not belong to {activeUser.Username}!");
                return;
            }

            if( !newDeck.SaveDeck(con.con) )
            {
                CreateResponse(Response.conflictCode, $"ERROR: Update or Insert of deck has not worked correctly!");
                return;
            }

            CreateResponse(Response.okCode, "Deck was updated successfully!");

        }

        internal void HandlePutUsersMessage(string user)
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r" || Authorization[1] != user)
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User a MTCG user to continue!");
                if (Authorization[1] != user)
                    CreateResponse(Response.unathorizedCode, $"ERROR: Only allowed to update data of User {Authorization[1]}");
                return;
            }

            Connector con = new Connector();

            CUser activeUser = new CUser(Authorization[1]);
            if (!activeUser.CheckLoggedIn(con.con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {activeUser.Username} is not logged in");
                return;
            }

            CUserData user_data = ConvertJsonUserDataContent();
            user_data.UpdateUserData(con.con, user);

            CreateResponse(Response.okCode, "User data successfully updated!");
        }
    }
}
