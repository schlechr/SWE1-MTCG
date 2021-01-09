using MonsterTradingCardGame.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Card
{
    public class CTradings
    {
        public string id { get; set; }
        public string card_to_trade { get; set; }
        public int req_cat_id { get; set; }
        public int req_min_dmg { get; set; }
        public Connector con { get; set; }

        public CTradings(string id, string cardToTrade, string type, int minDmg)
        {
            this.id = id;
            card_to_trade = cardToTrade;
            if (type == "Monster")
                req_cat_id = 1;
            else
                req_cat_id = 2;
            req_min_dmg = minDmg;
            con = new Connector();
        }
    }
}
