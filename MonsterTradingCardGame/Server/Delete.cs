using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.DB;
using MonsterTradingCardGame.User;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class Delete : REST
    {
        public Delete(string content, Dictionary<string, string> hv)
        {
            setREST(content, hv);
        }

        internal void HandleDeleteTradingsMessage(string trade_id)
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
            if(trade.CardToTrade.Equals(null))
            {
                CreateResponse(Response.badRequestCode, $"ERROR: Found no trade with ID {trade_id}");
                return;
            }

            if( trade.GetOwner() != activeUser.Username )
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: This trade do not belong to {activeUser.Username}");
                return;
            }

            trade.Delete();
            CreateResponse(Response.okCode, $"Trade {trade_id} successfully deleted");
        }
    }
}
