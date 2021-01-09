using Npgsql;
using System;
using System.Collections.Generic;

namespace MonsterTradingCardGame.DB
{
    public class Connector : IConnector
    {
        public NpgsqlConnection con { get; set; }

        public Connector()
        {
            con = new NpgsqlConnection("Host=localhost;Username=swe;Password=1234;Database=mtcg;");
            con.Open();
        }

        public int Update(string sql)
        {
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();
                
                Console.WriteLine("QUERY: " + sql);
                return cmd.ExecuteNonQuery();
            }
        }

        internal int SelectInt(string sql)
        {
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                        return rdr.GetInt32(0);
                }
            }
            return 0;
        }

        internal List<int> SelectListInt(int count, string sql)
        {
            List<int> res = new List<int>();

            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    for (int i = 0; i < count; i++)
                        if (!rdr.IsDBNull(i))
                            res.Add(rdr.GetInt32(i));
                }
            }

            if (res.Count != count)
                return null;
            else
                return res;
        }

        internal bool Insert(string sql)
        {
            try
            {
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("QUERY: " + sql);
                    return true;
                }
            } catch { return false; }
        }

        internal List<string> SelectListString(int count, string sql)
        {
            List<string> res = new List<string>();

            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    for (int i = 0; i < count; i++)
                        if (!rdr.IsDBNull(i))
                            res.Add(rdr.GetString(i));
                }
            }

            if (res.Count != count)
                return null;
            else
                return res;
        }

        public string SelectSingleString(string sql)
        {
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                        return rdr.GetString(0);
                }
            }
            return "";
        }

        internal Dictionary<int, string> SelectDictIntString(string sql)
        {
            Dictionary<int, string> res = new Dictionary<int, string>();

            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    res.Add(rdr.GetInt32(0), rdr.GetString(1));
                }
            }

            return res;
        }
    }
}
