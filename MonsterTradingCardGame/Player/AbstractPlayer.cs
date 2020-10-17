using System;

namespace MonsterTradingCardGame.Player
{
    public abstract class AbstractPlayer : IPlayer
    {
        public int coins { get; private set; }
        public int stack { get; private set; }

        protected AbstractPlayer(int coins)
        {
            this.coins = coins;
            this.stack = 0;
        }

        public int ShowMenu()
        {
            Console.WriteLine("You have {0} coins and {1} cards!", this.coins, this.stack);
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1 - Buy new package");

            Console.Write("Choose Number:");
            int i = Convert.ToInt32(Console.ReadLine());
            return i;
        }

        public void buyPackage()
        {
            if (this.coins < 5)
            {
                Console.WriteLine("You can't afford a new Pack!");
            }
            else
            {
                this.coins -= 5;
                this.stack += 4;
                Console.WriteLine("You got 4 new Cards!");
            }
        }
    }
}
