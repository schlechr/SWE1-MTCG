using MonsterTradingCardGame.Card;
using NUnit.Framework;


namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Monster i = new Monster();
            Assert.AreEqual( i.Lost(0), "Zero");
        }
    }
}