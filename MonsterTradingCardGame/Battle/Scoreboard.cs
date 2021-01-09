using MonsterTradingCardGame.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Battle
{
    public class Scoreboard
    {
        public string Username { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Loses { get; set; }
        public Connector con { get; set; }

        public Scoreboard (string user)
        {
            Username = user;
            con = new Connector();
            getScores();
        }

        private void getScores()
        {
            List<int> scores = con.SelectListInt(3, $"SELECT wins, draws, loses FROM scoreboard WHERE username = \'{Username}\'");

            if (scores == null)
            {
                con.Insert($"INSERT INTO scoreboard (username, wins, draws, loses) VALUES (\'{Username}\', 0, 0, 0)");
                this.addScores(new List<int>() { 0, 0, 0 });
            }
            else
            {
                this.addScores(scores);
            }
        }

        private void addScores(List<int> scores)
        {
            this.Wins = scores[0];
            this.Draws = scores[1];
            this.Loses = scores[2];
        }

        internal void Update()
        {
            con.Update($"UPDATE scoreboard SET wins = {Wins}, draws = {Draws}, loses = {Loses} WHERE username = \'{Username}\'");
        }

        internal string print()
        {
            string res = "";

            res += $"\nScoreboard of {Username}\n";
            res += $"Wins: {Wins}\n";
            res += $"Draws: {Draws}\n";
            res += $"Loses: {Loses}\n";

            return res;
        }
    }
}
