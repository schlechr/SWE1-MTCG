using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Battle
{
    class CBattle
    {
        public Connector con { get; set; }
        public Fighter FighterA { get; set; }
        public Fighter FighterB { get; set; }
        Dictionary<int, string> elements { get; set; }
        Dictionary<int, string> monstertypes { get; set; }
        Dictionary<int, string> categories { get; set; }
        

        public CBattle(string name)
        {
            FighterA = new Fighter(name);
            con = new Connector();
        }

        internal bool FindOpponent()
        {
            FighterB = new Fighter( con.SelectSingleString($"SELECT username FROM decks WHERE fight_lock = true AND username != \'{FighterA.Username}\'") );

            if (FighterB.Username == "")
                return false;

            return true;
        }

        internal bool SetFighterAvailable()
        {
            int res = con.Update($"UPDATE decks SET fight_lock = true WHERE username = \'{FighterA.Username}\'");
            if (res == 0)
            {
                Console.WriteLine("ERROR: Update was not working!!!");
                return false;
            }
            return true;
        }

        internal bool PrepareFighters()
        {
            if (!FighterA.Prepare(con))
                return false;
            if (!FighterB.Prepare(con))
                return false;

            return true;
        }

        internal bool PrepareEnvironment()
        {
            elements     = con.SelectDictIntString("SELECT element_id, name FROM elements");
            categories   = con.SelectDictIntString("SELECT cat_id, name FROM categories");
            monstertypes = con.SelectDictIntString("SELECT mt_id, name FROM monstertypes");

            if (elements == null || categories == null || monstertypes == null)
                return false;

            return true;
        }

        internal string Start()
        {
            string result_string = PreFightComments();

            for (int i = 0; i < 100; i++)
            {
                int a = FighterA.ChooseCard();
                int b = FighterB.ChooseCard();

                int res = this.NewRound(FighterA.Deck.cards[a], FighterB.Deck.cards[b], ref result_string);
                result_string += this.EvaluateResult(res, a, b);

                if( FighterA.Deck.cards.Count == 0 )
                {
                    result_string += this.CrownWinner(FighterB, FighterA);
                    return result_string;
                }
                else if(FighterB.Deck.cards.Count == 0)
                {
                    result_string += this.CrownWinner(FighterA, FighterB);
                    return result_string;
                }
            }

            return result_string;
        }

        private string CrownWinner(Fighter winner, Fighter looser)
        {
            winner.FighterScore.Wins++;
            looser.FighterScore.Loses++;

            winner.FighterScore.Update();
            looser.FighterScore.Update();

            
            winner.GetCards(con, looser);

            con.Update($"UPDATE decks SET fight_lock = false WHERE username = \'{winner.Username}\'");

            return $"\nBattle is over! Congrats to {winner.Username}!!!\n";
        }

        private string PreFightComments()
        {
            string res = "";
            res += "LET'S GOOOOOO!!!\n\n";
            res += $"{ FighterA.Deck.cards[0].name,22} -  { FighterA.Deck.cards[0].damage,2}     |    { FighterB.Deck.cards[0].damage,2} - { FighterB.Deck.cards[0].name}\n";
            res += $"{ FighterA.Deck.cards[1].name,22} -  { FighterA.Deck.cards[1].damage,2}     |    { FighterB.Deck.cards[1].damage,2} - { FighterB.Deck.cards[1].name}\n";
            res += $"{ FighterA.Deck.cards[2].name,22} -  { FighterA.Deck.cards[2].damage,2}     |    { FighterB.Deck.cards[2].damage,2} - { FighterB.Deck.cards[2].name}\n";
            res += $"{ FighterA.Deck.cards[3].name,22} -  { FighterA.Deck.cards[3].damage,2}     |    { FighterB.Deck.cards[3].damage,2} - { FighterB.Deck.cards[3].name}\n";
            res += $"\n{FighterA.Username,30}  vs.  {FighterB.Username}\n\n";

            return res;
        }

        private string EvaluateResult(int res, int a, int b)
        {
            if (res == 1)
                return this.TransferCard(FighterA, FighterB, b);
            else if (res == 2)
                return this.TransferCard(FighterB, FighterA, a);
            else
                return "This Round ends in a draw\n";
        }

        private string TransferCard(Fighter winner, Fighter looser, int i)
        {
            winner.Deck.cards.Add(looser.Deck.cards[i]);
            looser.Deck.cards.RemoveAt(i);

            return $"Winner of this round is {winner.Username}\n";
        }

        private int NewRound(CCard cardA, CCard cardB, ref string res_str)
        {
            double dmgA = cardA.damage;
            double dmgB = cardB.damage;

            if ((cardA.cat_id == 2 && cardB.cat_id == 2) ||
                (cardA.cat_id != cardB.cat_id))
            {
                dmgA = this.CalculateDmgByElements( dmgA, cardA.element_id, cardB.element_id );
                dmgB = this.CalculateDmgByElements(dmgB, cardB.element_id, cardA.element_id);
            }


            res_str += $"{cardA.name,20} | {dmgA,4}({cardA.damage,2}) vs. {dmgB,4}({cardB.damage,2}) | {cardB.name}\n";
            if (this.CheckSpecialties(cardA.mt_id, cardA.element_id, cardB.mt_id, cardB.element_id, ref res_str))
            {
                return 1;
            }

            if (dmgA > dmgB)
                return 1;
            else if (dmgA < dmgB)
                return 2;
            else
                return 0;
        }

        private bool CheckSpecialties(int mt_id1, int element_id1, int mt_id2, int element_id2, ref string res_str)
        {
            string cat = monstertypes[mt_id1];
            string ele = elements[element_id1];
            string opp = monstertypes[mt_id2];
            string opp_ele = elements[element_id2];

            if (cat.Equals("Dragon"))
            {
                if (opp.Equals("Goblin"))
                {
                    res_str += "Goblins are too afraid of Dragons to attack! Automatic WIN!\n";
                    return true;
                }
            }
            else if (cat.Equals("Wizzard"))
            {
                if (opp.Equals("Ork"))
                {
                    res_str += "Wizzards can control Orks so they are not able to damage them! Automatic WIN!\n";
                    return true;
                }
            }
            else if (cat.Equals("Spell") && ele.Equals("Water"))
            {
                if (opp.Equals("Knight"))
                {
                    res_str += "The armor of Knights is so heavy that WaterSpells make them drown instantly! Automatic WIN!\n";
                    return true;
                }
            }
            else if(cat.Equals("Kraken"))
            {
                if (opp.Equals("Spell"))
                {
                    res_str += "The Kraken is immune against spells! Automatic WIN!\n";
                    return true;
                }
            }
            else if(cat.Equals("Elf") && ele.Equals("Fire"))
            {
                if (opp.Equals("Dragon"))
                {
                    res_str += "FireElves know Dragons since they were little and can evade their attacks! Automatic WIN!\n";
                    return true;
                }
            }

            return false;
        }

        private double CalculateDmgByElements(double dmg, int ele1, int ele2)
        {
            double res = dmg;
            switch(ele1)
            {
                case 1: //Normal
                    switch(ele2)
                    {
                        case 1:
                            break;
                        case 2:
                            res *= 2;
                            break;
                        case 3:
                            res /= 2;
                            break;
                    }
                    break;
                case 2: //Water
                    switch(ele2)
                    {
                        case 1:
                            res /= 2;
                            break;
                        case 2:
                            break;
                        case 3:
                            res *= 2;
                            break;
                    }
                    break;
                case 3: //Fire
                    switch(ele2)
                    {
                        case 1:
                            res *= 2;
                            break;
                        case 2:
                            res /= 2;
                            break;
                        case 3:
                            break;
                    }
                    break;
            }

            return res;
        }
    }
}
