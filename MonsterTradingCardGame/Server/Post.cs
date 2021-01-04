using MonsterTradingCardGame.User;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class Post
    {
        public string ContentType { get; set; }
        public string Authorization { get; set; }
        public string Content { get; set; }
        public string RespCode { get; set; }
        public string Resp { get; set; }

        public Post( string content, Dictionary<string, string> hv )
        {

            ContentType = CheckDictonaryKey(hv, "Content-Type");
            Authorization = CheckDictonaryKey(hv, "Authorization");
            Content = content;
            RespCode = "";
            Resp = "";
        }

        private string CheckDictonaryKey(Dictionary<string, string> hv, string v)
        {
            if (hv.ContainsKey(v))
                return hv[v];
            else
                return "";
        }

        public void HandlePostUsersMessage()
        {
            TUser newUser = ConvertJsonUserContent();

            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            //If user already exists in DB exit methode
            if (CompareDbEntries(con, "SELECT username FROM users", newUser.Username) )
                return;

            newUser.AddToDb(con);

            RespCode = "200";
            Resp = "Okay";
            return;
        }

        private bool CompareDbEntries(NpgsqlConnection con, string sql, string cmp)
        {
            using (var cmd = new NpgsqlCommand(sql, con))
                using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    while (rdr.Read())
                    {
                        if (rdr.GetString(0) == cmp)
                            return true;
                    }
            return false;
        }

        public void HandlePostSessionsMessage()
        {
            TUser loginUser = ConvertJsonUserContent();

            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            //If User is not found in DB check, exit methode
            if (!CompareDbEntries(con, "SELECT username FROM users", loginUser.Username))
                return;
        }

        private TUser ConvertJsonUserContent()
        {
            TUser tmp_res = new TUser();

            if ( ContentType == "application/json\r")
            {
                tmp_res = JsonConvert.DeserializeObject<TUser>(Content);

                Console.WriteLine(tmp_res.Username);
                Console.WriteLine(tmp_res.Password);
            }

            Console.WriteLine(Content);

            return tmp_res;
        }
    }
}
