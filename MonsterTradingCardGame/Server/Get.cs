using MonsterTradingCardGame.User;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class Get : REST
    {
        public Get(string content, Dictionary<string, string> hv)
        {
            setREST(Content, hv);
        }

        public void HandleGetCardsMessage()
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User a MTCG user to continue!");
                return;
            }

            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            CUser activeUser = new CUser(Authorization[1]);
            if (!activeUser.CheckLoggedIn(con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {activeUser.Username} is not logged in");
                return;
            }

            string str_cards = activeUser.PrintAcquiredCards(con);
            if ( str_cards.Equals(""))
            {
                CreateResponse(Response.noContentCode, $"ERROR: {activeUser.Username} do not have any cards");
                return;
            }

            CreateResponse(Response.createCode, str_cards);
            //Console.WriteLine(str_cards);
        }
    }
}
