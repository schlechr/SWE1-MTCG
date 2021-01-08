using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.User;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class REST
    {
        public string ContentType { get; set; }
        public string[] Authorization { get; set; }
        public string Content { get; set; }
        public string RespCode { get; set; }
        public string Resp { get; set; }

        public void setREST(string content, Dictionary<string, string> hv)
        {

            ContentType = CheckDictonaryKey(hv, "Content-Type");
            Authorization = DecomposeAuth(CheckDictonaryKey(hv, "Authorization"));
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
        protected CUser ConvertJsonUserContent()
        {
            CUser tmp_res = new CUser();

            if (ContentType == "application/json\r")
                tmp_res = JsonConvert.DeserializeObject<CUser>(Content);

            return tmp_res;
        }

        protected List<CCard> ConvertJsonCardsContent()
        {
            List<CCard> tmp_res = new List<CCard>();

            if (ContentType == "application/json\r")
            {
                tmp_res = JsonConvert.DeserializeObject<List<CCard>>(Content);
            }

            return tmp_res;
        }

        protected List<string> ConvertJsonDeckContent()
        {
            List<string> tmp_res = new List<string>();

            if (ContentType == "application/json\r")
            {
                tmp_res = JsonConvert.DeserializeObject<List<string>>(Content);
            }

            return tmp_res;
        }

        protected void CreateResponse(string code, string msg)
        {
            RespCode = code;
            Resp = msg;
            Console.WriteLine("OUTPUT: " + Resp);
        }

        public void CreateFinalResponse(ref string code, ref string msg)
        {
            if (RespCode != "")
            {
                code = RespCode;
                msg = Resp;
            }
        }

        protected bool CompareDbEntries(NpgsqlConnection con, string sql, string cmp)
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
    }
}
