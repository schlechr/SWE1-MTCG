using Npgsql;

namespace MonsterTradingCardGame.DB
{
    public interface IConnector
    {
        NpgsqlConnection con { get; set; }
    }
}
