using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Card
{
    public class CDeck
    {
        public List<CCard> cards { get; set; }

        public CDeck(NpgsqlConnection con, List<string> card_id)
        {
            cards = new List<CCard>();
            //CCard x = new CCard(card_id[0]);
            foreach (string c in card_id)
                cards.Add(new CCard(c));

            foreach (CCard c in cards)
                c.CompleteCardInformation(con);
        }

        public CDeck(NpgsqlConnection con, string user)
        {
            string sql = $"SELECT card1, card2, card3, card4 FROM decks WHERE username = \'{user}\'";
        }

        internal string GetCardsOwner()
        {
            string user = "";

            foreach( CCard c in cards)
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
            string sql = $"UPDATE decks SET card1 = \'{cards[0].id}\', card2 = \'{cards[1].id}\', " +
                $"card3 = \'{cards[2].id}\', card4 = \'{cards[3].id}\' WHERE username = \'{cards[0].username}\' AND fight_lock = false";

            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();

                Console.WriteLine("QUERY: " + sql);
                if (cmd.ExecuteNonQuery() == 0)
                    if (!InsertNewDeck(con))
                        return false;
            }
            return true;
        }

        private bool InsertNewDeck(NpgsqlConnection con)
        {
            string sql = $"INSERT INTO decks(username, card1, card2, card3, card4, fight_lock) " +
                $"VALUES(\'{cards[0].username}\', \'{cards[0].id}\', \'{cards[1].id}\', \'{cards[2].id}\', \'{cards[3].id}\', false)";
            try
            {
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("QUERY: " + sql);
                    return true;
                }
            } catch { Console.WriteLine("ERROR: Deck can not be configured as it is locked right now!"); return false; }
        }
    }
}
