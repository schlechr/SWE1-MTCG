using MonsterTradingCardGame.Player;
using NUnit.Framework;

namespace MonsterTradingCardGame.Player
{
    public class PlayerTest
    {
        [Test]
        public void TestBuy_newPackage()
        {
            IPlayer user = new NewPlayer();

            user.buyPackage();

            Assert.AreEqual(15, user.coins);
        }

        [Test]
        public void TestBuy_noMoney()
        {
            IPlayer user = new NewPlayer();

            for (int i = 0; i < 6; i++) { user.buyPackage(); }

            Assert.AreEqual(0, user.coins);
            Assert.AreEqual(16, user.stack);
        }
    }
}