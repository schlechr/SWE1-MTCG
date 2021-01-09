using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Battle
{
    public class Fighter
    {
        public string Username { get; set; }
        public CDeck Deck { get; set; }
        public Scoreboard FighterScore { get; set; }

        public Fighter(string name)
        {
            Username = name;
            FighterScore = new Scoreboard(name);
        }

        internal bool Prepare(Connector con)
        {
            List<string> cards = con.SelectListString(4, $"SELECT card1, card2, card3, card4 FROM decks WHERE username = \'{Username}\'");
            if (cards == null)
                return false;

            Deck = new CDeck(con.con, cards);
            return true;
        }

        internal int ChooseCard()
        {
            var random = new Random();
            return random.Next(Deck.cards.Count);
        }

        internal void GetCards(Fighter looser) //TODO
        {
            throw new NotImplementedException();
        }
    }
}
