namespace MonsterTradingCardGame.Card
{
    public interface ICard
    {
        string Name { get; }
        int damage { get; }
        int element { get; }
        bool isMonster();
        string Lost(int i);
    }
    
}
