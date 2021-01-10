using MonsterTradingCardGame.DB;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Card
{
    public class CTradings
    {
        public string Id { get; set; }
        public string CardToTrade { get; set; }
        public string Type { get; set; }
        public int Type_id { get; set; }
        public int MinimumDamage { get; set; }
        public Connector con { get; set; }

        public CTradings()
        {
            con = new Connector();
        }

        public CTradings(string id)
        {
            Id = id;
            con = new Connector();
            completeTradingDetails();
        }

        private void completeTradingDetails()
        {
            string sql = $"SELECT card_to_trade, req_type, req_min_dmg FROM trades WHERE trade_id = \'{Id}\'";
            using (var cmd = new NpgsqlCommand(sql, con.con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    CardToTrade = rdr.GetString(0);
                    Type = rdr.GetString(1);
                    if (Type.ToLower() == "monster")
                        Type_id = 1;
                    else
                        Type_id = 2;
                    MinimumDamage = rdr.GetInt32(2);
                }
            }
        }

        public CTradings(string id, string cardToTrade, string type, int minDmg)
        {
            Id = id;
            CardToTrade = cardToTrade;
            Type = type;
            if (Type.ToLower() == "monster")
                Type_id = 1;
            else
                Type_id = 2;
            MinimumDamage = minDmg;
            con = new Connector();
        }

        internal void Delete()
        {
            con.Delete($"DELETE FROM trades WHERE trade_id = \'{Id}\'");
        }

        internal string GetOwner()
        {
            string real_owner = con.SelectSingleString($"SELECT username FROM cards WHERE card_id = \'{CardToTrade}\'");
            return real_owner;
        }

        public CTradings(string id, string cardToTrade, int type, int minDmg)
        {
            Id = id;
            CardToTrade = cardToTrade;
            Type_id = type;
            if (Type_id == 1)
                Type = "monster";
            else
                Type = "spell";
            MinimumDamage = minDmg;
            con = new Connector();
        }

        public List<CTradings> GetTradings()
        {
            List<CTradings> res = new List<CTradings>();

            string sql = "SELECT trade_id, card_to_trade, req_type, req_min_dmg FROM trades";
            using (var cmd = new NpgsqlCommand(sql, con.con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    res.Add(new CTradings(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3)));
                }
            }

            return res;
        }

        public string Post(string username)
        {
            string sql = $"SELECT username FROM cards WHERE card_id = \'{CardToTrade}\'";
            string cmp = con.SelectSingleString(sql);
            if (cmp != username)
                return "ERROR: Trade not possible! You can only trade your own cards!\n";

            sql = $"SELECT card1, card2, card3, card4 FROM decks WHERE username = \'{username}\'";
            List<string> cmp_list = con.SelectListString(4, sql);
            if( cmp_list != null )
                foreach (string x in cmp_list)
                    if (x == CardToTrade)
                        return "ERROR: Trade not possible! Card is used in your deck!\n";

            sql = $"INSERT INTO trades (trade_id, card_to_trade, req_type, req_min_dmg) " +
                $"VALUES(\'{Id}\', \'{CardToTrade}\', \'{Type}\', {MinimumDamage})";
            if (!con.Insert(sql))
                return "ERROR: This Trade is already active!\n";

            return "DONE! Trade is now active and available for other Users\n"; ;
        }

        public string Print()
        {
            string res = $"ID: {Id} - Card To Trade: {CardToTrade} - Requested Type: {Type} - Requested Minimum Damage: {MinimumDamage}\n";
            return res;
        }

        internal bool Trade(CCard trade_card, string tradeOwner)
        {
            if (trade_card.damage < MinimumDamage)
                return false;
            if (trade_card.cat_id != Type_id)
                return false;

            con.Update($"UPDATE cards SET username = \'{trade_card.username}\' WHERE card_id = \'{CardToTrade}\'");
            con.Update($"UPDATE cards SET username = \'{tradeOwner}\' WHERE card_id = \'{trade_card.id}\'");
            con.Delete($"DELETE FROM trades WHERE trade_id = \'{Id}\'");
            return true;
        }
    }
}
