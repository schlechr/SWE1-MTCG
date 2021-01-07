using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.User;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class Post
    {
        public string ContentType { get; set; }
        public string[] Authorization { get; set; }
        public string Content { get; set; }
        public string RespCode { get; set; }
        public string Resp { get; set; }

        public Post( string content, Dictionary<string, string> hv )
        {

            ContentType = CheckDictonaryKey(hv, "Content-Type");
            Authorization = DecomposeAuth( CheckDictonaryKey(hv, "Authorization") );
            Content = content;
            RespCode = "";
            Resp = "";
        }

        //initalize functions
        private string[] DecomposeAuth(string v)
        {
            //Authorization Format: "Basic username-mtcgToken"
            //[0]: Basic, [1]: username, [2]: mtcgToken
            string[] res = v.Split(new char[] { ' ', '-' });

            return res;
        }

        private string CheckDictonaryKey(Dictionary<string, string> hv, string v)
        {
            if (hv.ContainsKey(v))
                return hv[v];
            else
                return "";
        }

        //Message handling functions
        public void HandlePostUsersMessage()
        {
            CUser newUser = ConvertJsonUserContent();

            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            //If user already exists in DB exit methode
            if (CompareDbEntries(con, "SELECT username FROM users", newUser.Username) )
            {
                RespCode = Response.conflictCode;
                Resp = $"User {newUser.Username} already exists!";
                Console.WriteLine(Resp);
                return;
            }
            
            newUser.AddToDb(con);

            RespCode = Response.okCode;
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

            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            CUser adminUser = new CUser(Authorization[1]);
            if (!adminUser.CheckLoggedIn(con))
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

            int package_id = cards[0].GetNextPackageId( con );
            foreach( CCard tmp_card in cards )
            {
                tmp_card.FinishCardDetails( con, package_id );
                tmp_card.AddToDb(con);
            }

            CreateResponse( Response.okCode, "Admin is logged in" );
            return;
        }

        internal void HandlePostTransactionsPackagesMessage()
        {
            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            CUser buyUser = new CUser(Authorization[1]);
            if (!buyUser.CheckLoggedIn(con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {buyUser.Username} is not logged in");
                return;
            }

            if (!buyUser.CheckCoinsForPurchase(con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {buyUser.Username} do not have enough coins");
                return;
            }

            if (!buyUser.PurchasePackage(con))
            {
                CreateResponse(Response.conflictCode, $"ERROR: No package available at the moment");
                return;
            }

            CreateResponse(Response.okCode, $"Package was purchased successfully");
        }

        public void HandlePostSessionsMessage()
        {
            CUser loginUser = ConvertJsonUserContent();

            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            //If User is not found in DB check, exit methode
            if (!CompareDbEntries(con, "SELECT username FROM users", loginUser.Username))
            {
                RespCode = Response.conflictCode;
                Resp = $"User {loginUser.Username} not found!";
                Console.WriteLine(Resp);
                return;
            }

            if ( loginUser.setActive(con) == 1 )
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

        private bool CompareDbEntries(NpgsqlConnection con, string sql, string cmp)
        {
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    if (rdr.GetString(0) == cmp)
                        return true;
                }
            }
            return false;
        }

        private CUser ConvertJsonUserContent()
        {
            CUser tmp_res = new CUser();

            if ( ContentType == "application/json\r")
                tmp_res = JsonConvert.DeserializeObject<CUser>(Content);

            return tmp_res;
        }

        private List<CCard> ConvertJsonCardsContent()
        {
            List<CCard> tmp_res = new List<CCard>();

            if (ContentType == "application/json\r")
            {
                tmp_res = JsonConvert.DeserializeObject<List<CCard>>(Content);
            }

            return tmp_res;
        }

        private void CreateResponse( string code, string msg )
        {
            RespCode = code;
            Resp = msg;
            Console.WriteLine("OUTPUT: " + Resp);
        }
    }
}
