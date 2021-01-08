using MonsterTradingCardGame.Server;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.User
{
    public class CUser : IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public CUser(string user = "", string pw = "")
        {
            Username = user;
            Password = pw;
        }

        public void AddToDb(NpgsqlConnection con)
        {
            try
            {
                string sql = $"INSERT INTO users(username, password, coins, active) VALUES(\'{Username}\', \'{Password}\', 20, false)";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Prepare();

                    Console.WriteLine("QUERY: " + sql);
                    cmd.ExecuteNonQuery();
                }
            } 
            catch { Console.WriteLine("ERROR with executing the last query!"); }
        }

        public int setActive(NpgsqlConnection con)
        {
            string sql = $"UPDATE users SET active = true WHERE username = \'{Username}\' AND password = \'{Password}\'";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();

                Console.WriteLine("QUERY: " + sql);
                return cmd.ExecuteNonQuery();
            }
        }

        public string PrintAcquiredCards(NpgsqlConnection con)
        {
            string res = "";
            string sql = $"SELECT name, damage FROM cards WHERE username = \'{Username}\'";

            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    res += $"{rdr.GetString(0)} -> Damage: {rdr.GetDouble(1)}\n";
                }
            }
            return res;
        }

        public bool CheckLoggedIn(NpgsqlConnection con)
        {
            string sql = $"SELECT active FROM users WHERE username = \'{Username}\'";
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    return rdr.GetBoolean(0);
                }
            }

            return false;
        }

        internal bool CheckCoinsForPurchase(NpgsqlConnection con)
        {
            string sql = $"SELECT coins FROM users WHERE username = \'{Username}\'";
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    if (rdr.GetInt32(0) >= 5)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        internal bool PurchasePackage(NpgsqlConnection con)
        {
            int tmp_package_id = 0;

            string sql = "SELECT count(*), package_id FROM cards WHERE username = \'admin\' GROUP BY package_id";
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    if (rdr.GetInt32(0) == 5)
                        tmp_package_id = rdr.GetInt32(1);
                }
            }

            if (tmp_package_id == 0)
                return false;

            sql = $"UPDATE users SET coins = coins-5 WHERE username = \'{Username}\'";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();
                Console.WriteLine("QUERY: " + sql);
                cmd.ExecuteNonQuery();
            }

            sql = $"UPDATE cards SET username = \'{Username}\' WHERE package_id = {tmp_package_id}";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();
                Console.WriteLine("QUERY: " + sql);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
    }
}
