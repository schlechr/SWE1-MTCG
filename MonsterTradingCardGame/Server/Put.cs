﻿using MonsterTradingCardGame.Card;
using MonsterTradingCardGame.User;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTradingCardGame.Server
{
    public class Put : REST
    {
        public Put(string content, Dictionary<string,string> hv)
        {
            setREST(content, hv);
        }

        internal void HandlePutDeckMessage()
        {
            if (Authorization.Length < 2 || Authorization[2] != "mtcgToken\r")
            {
                CreateResponse(Response.forbiddenCode, "ERROR: User a MTCG user to continue!");
                return;
            }

            var cs = "Host=localhost;Username=swe;Password=1234;Database=mtcg;";
            var con = new NpgsqlConnection(cs);
            con.Open();

            CUser activeUser = new CUser(Authorization[1]);
            if (!activeUser.CheckLoggedIn(con))
            {
                CreateResponse(Response.unathorizedCode, $"ERROR: {activeUser.Username} is not logged in");
                return;
            }

            CDeck newDeck = new CDeck(con, ConvertJsonDeckContent());
            if( newDeck.cs.Count != 4 )
            {
                CreateResponse(Response.conflictCode, $"ERROR: There must be 4 cards declared for the deck!");
                return;
            }

            if( newDeck.GetCardsOwner() != activeUser.Username )
            {
                CreateResponse(Response.forbiddenCode, $"ERROR: One or more cards do not belong to {activeUser.Username}!");
                return;
            }

            if( !newDeck.SaveDeck(con) )
            {
                CreateResponse(Response.conflictCode, $"ERROR: Update or Insert of deck has not worked correctly!");
                return;
            }

            CreateResponse(Response.okCode, "Deck was updated successfully!");

        }
    }
}