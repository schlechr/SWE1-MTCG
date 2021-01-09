using MonsterTradingCardGame.Battle;
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
            setREST(content, hv);
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

        internal void HandleGetDeckMessage(bool plain)
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

            List<string> deck_cards = activeUser.GetDeck(con);
            string res = "";

            if (!plain)
            {
                res = $"1: \'{deck_cards[0]}\'\n2: \'{deck_cards[1]}\'\n3: \'{deck_cards[2]}\'\n4: \'{deck_cards[3]}\'";
                CreateResponse(Response.okCode, res);
                return;
            }

            string sql = "SELECT name, damage FROM cards WHERE card_id IN " +
                $"(\'{deck_cards[0]}\', \'{deck_cards[1]}\', \'{deck_cards[2]}\', \'{deck_cards[3]}\')";
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                int i = 1;
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    res += $"{i}: {rdr.GetString(0)} -> Damage: {rdr.GetDouble(1)}\n";
                    i++;
                }
                CreateResponse(Response.okCode, res);
                return;
            }

        }

        internal void HandleGetUsersMessage(string user)
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r" || Authorization[1] != user )
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User must be a MTCG user to continue!");
                if (Authorization[1] != user)
                    CreateResponse(Response.unathorizedCode, $"ERROR: Only allowed to see data of User {Authorization[1]}");
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

            CUserData cud = new CUserData(con, user);
            CreateResponse(Response.okCode, cud.DataToString());
        }

        internal void HandleGetStatsMessage()
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User must be a MTCG user to continue!");
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

            CUserStats stats = new CUserStats(activeUser.Username);
            string res = stats.print();
            CreateResponse(Response.okCode, res);
        }

        internal void HandleGetScoreMessage()
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User must be a MTCG user to continue!");
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

            Scoreboard score = new Scoreboard(activeUser.Username);
            string res = score.print();
            CreateResponse(Response.okCode, res);
        }

        internal void HandleGetTradingsMessage()
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User must be a MTCG user to continue!");
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

            while(true)
            {

            }
        }
    }
}
