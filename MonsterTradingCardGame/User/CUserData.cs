using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.User
{
    public class CUserData
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public CUserData()
        {
            Name = Bio = Image = "";
        }

        public CUserData(NpgsqlConnection con, string user)
        {
            Name = Bio = Image = "";
            string sql = $"SELECT name, bio, image FROM users WHERE username = \'{user}\'";
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                        Name = rdr.GetString(0);
                    if (!rdr.IsDBNull(1))
                        Bio = rdr.GetString(1);
                    if (!rdr.IsDBNull(2))
                        Image = rdr.GetString(2);
                }
            }
        }

        internal string DataToString()
        {
            string res = $"\nName: {Name}\nBio: {Bio}\nImage: {Image}";
            return res;
        }

        internal void UpdateUserData(NpgsqlConnection con, string user)
        {
            string sql = $"UPDATE users SET name = \'{Name}\', bio = \'{Bio}\', image = \'{Image}\' WHERE username = \'{user}\'";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Prepare();

                Console.WriteLine("QUERY: " + sql);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
