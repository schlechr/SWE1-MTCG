using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Card
{
    public class CDeck
    {
        public List<CCard> cs { get; set; }

        public CDeck(NpgsqlConnection con, List<string> card_id)
        {
            cs = new List<CCard>();
            //CCard x = new CCard(card_id[0]);
            foreach (string c in card_id)
                cs.Add(new CCard(c));

            foreach (CCard c in cs)
                c.CompleteCardInformation(con);
        }

        internal string GetCardsOwner()
        {
            string user = "";

            foreach( CCard c in cs )
            {
                if (user != "" && user != c.username)
                    return "";
                user = c.username;
            }

            return user;
        }

        internal bool SaveDeck(NpgsqlConnection con)
        {
            //string sql = $"SELECT username FROM decks WHERE username = \'{cs[0].username}\'";
            string sql = $"UPDATE decks SET card1 = \'{cs[0].id}\', card2 = \'{cs[1].id}\', " +
                $"card3 = \'{cs[2].id}\', card4 = \'{cs[3].id}\' WHERE username = \'{cs[0].username}\'";

            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();

                Console.WriteLine("QUERY: " + sql);
                if (cmd.ExecuteNonQuery() == 0)
                    InsertNewDeck(con);
            }
            return true;
        }

        private void InsertNewDeck(NpgsqlConnection con)
        {
            string sql = $"INSERT INTO decks(username, card1, card2, card3, card4) " +
                $"VALUES(\'{cs[0].username}\', \'{cs[0].id}\', \'{cs[1].id}\', \'{cs[2].id}\', \'{cs[3].id}\')";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();

                Console.WriteLine("QUERY: " + sql);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
