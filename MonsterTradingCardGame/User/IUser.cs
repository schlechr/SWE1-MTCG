using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.User
{
    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }

        void AddToDb(NpgsqlConnection con);
        int setActive(NpgsqlConnection con);
    }
}
