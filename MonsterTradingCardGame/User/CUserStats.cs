using MonsterTradingCardGame.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.User
{
    public class CUserStats
    {
        public string Username { get; set; }
        public double WinLoseRatio { get; set; }
        public int CardsCount { get; set; }
        public Connector con { get; set; }

        public CUserStats (string user)
        {
            Username = user;
            con = new Connector();
            WinLoseRatio = this.calcWinLoseRatio();
            CardsCount = this.getCardsCount();
        }

        private int getCardsCount()
        {
            return con.SelectInt($"SELECT count(*) FROM cards WHERE username = \'{Username}\'");
        }

        private double calcWinLoseRatio()
        {
            List<int> WinLose = con.SelectListInt(2, $"SELECT wins, loses FROM scoreboard WHERE username = \'{Username}\'");

            if (WinLose.Equals(null))
                return 0;

            return ((double)WinLose[0] / (double)WinLose[1]);
        }

        internal string print()
        {
            string res = "";

            res += $"\nUser Stats of {Username}\n";
            res += $"Amount of Cards: {CardsCount}\n";
            res += $"Win/Lose Ratio: {WinLoseRatio}\n";

            return res;
        }
    }
}
