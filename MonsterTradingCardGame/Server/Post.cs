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
    }
}
