namespace MonsterTradingCardGame.Card
{
    public interface ICard
    {
        string id { get; set; }
        string name { get; set; }
        double damage { get; set; }
        int cat_id { get; set; }
        string username { get; set; }
        int package_id { get; set; }
        int element_id { get; set; }
        int mt_id { get; set; }
    }
    
}
