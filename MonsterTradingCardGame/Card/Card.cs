namespace MonsterTradingCardGame.Card
{
    public class Card : ICard
    {
        public string Name { get; set; }
        public int damage { get; private set; }
        public int element { get; set; }

        public bool isMonster()
        {
            throw new System.NotImplementedException();
        }

        protected Card()
        {
            this.Name = "Test";

        }

        public string Lost(int i)
        {
            if (i == 0)
            {
                return ("Zero");
            }
            else
            {
                return ("NotZero");
            }
        }
    }
}
