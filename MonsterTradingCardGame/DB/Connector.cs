using Npgsql;
using System;

namespace MonsterTradingCardGame.DB
{
    public class Connector : IConnector
    {
        public NpgsqlConnection con { get; set; }

        public Connector( string con_string )
        {
            con = new NpgsqlConnection( con_string );
            con.Open();
        }

        public int UpdateDB(string sql)
        {
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();
                
                Console.WriteLine("QUERY: " + sql);
                return cmd.ExecuteNonQuery();
            }
        }

    }
}
