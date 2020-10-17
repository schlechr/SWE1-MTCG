namespace MonsterTradingCardGame.Player
{
    public interface IPlayer
    {
        int coins { get; }
        int stack { get; }

        int ShowMenu();
        void buyPackage();
    }
}
