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

        public void AddToDb(NpgsqlConnection con)
        {
            string sql = $"INSERT INTO users(username, password, coins, active) VALUES(\'{Username}\', \'{Password}\', 20, false)";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();

                Console.WriteLine("QUERY: " + sql);
                cmd.ExecuteNonQuery();
            }
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
    }
}
