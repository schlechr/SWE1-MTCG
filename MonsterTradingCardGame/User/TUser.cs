using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.User
{
    public class TUser
    {
        public string Username { get; set; }
        public string Password { get; set; }

        internal void AddToDb(NpgsqlConnection con)
        {
            string sql = "INSERT INTO users(username, password) VALUES(@username, @password)";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("username", Username);
                cmd.Parameters.AddWithValue("password", Password);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
                Console.WriteLine("row inserted");
            }
        }
    }
}
