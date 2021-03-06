﻿using MonsterTradingCardGame.DB;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonsterTradingCardGame.Card
{
    public class CCard : ICard
    {
        public string id { get; set; }
        public string name { get; set; }
        public double damage { get; set; }
        public int cat_id { get; set; }
        public string username { get; set; }
        public int package_id { get; set; }
        public int element_id { get; set; }
        public int mt_id { get; set; }

        public CCard(string id)
        {
            this.id = id;
        }

        internal int GetNextPackageId(NpgsqlConnection con)
        {
            string sql = "SELECT max(package_id) FROM cards";
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    if( !rdr.IsDBNull(0) )
                        return rdr.GetInt32(0) + 1;
                }
            }

            return 1;
        }

        internal void FinishCardDetails(NpgsqlConnection con, int package_id)
        {
            try
            {
                Console.WriteLine($"Finish Card Details for {this.name}!");

                string sql = "SELECT mt_id, name FROM monstertypes";
                using (var cmd = new NpgsqlCommand(sql, con))
                using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                {
                    Console.WriteLine("QUERY: " + sql);
                    while (rdr.Read())
                    {
                        if (this.name.Contains(rdr.GetString(1)))
                        {
                            this.mt_id = rdr.GetInt32(0);
                            break;
                        }
                    }
                    Debug.WriteLine($"Done with Monstertype for {this.name} -> {this.mt_id}!");
                }

                if (this.mt_id == 1)
                    this.cat_id = 2; //Spell Card
                else
                    this.cat_id = 1; //Monster Card

                Debug.WriteLine($"Done with Categorie for {this.name} -> {this.cat_id}!");

                sql = "SELECT element_id, name FROM elements";
                using (var cmd = new NpgsqlCommand(sql, con))
                using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                {
                    Console.WriteLine("QUERY: " + sql);
                    while (rdr.Read())
                    {
                        if (this.name.Contains(rdr.GetString(1)) || 
                            (this.name == "Dragon" && rdr.GetString(1) == "Fire") )
                        {
                            this.element_id = rdr.GetInt32(0);
                            break;
                        }
                    }
                    if (this.element_id == 0)
                        this.element_id = 1;
                    Debug.WriteLine($"Done with Element for {this.name} -> {this.element_id}!");
                }


                this.username = "admin";
                Debug.WriteLine($"Done with Username for {this.name} -> {this.username}!");

                this.package_id = package_id;
                Debug.WriteLine($"Done with Package for {this.name} -> {this.package_id}!");
                Console.WriteLine("-----------------------------------------------------------");
            }
            catch { Console.WriteLine("ERROR with executing the last query!"); }
        }

        internal void AddToDb(NpgsqlConnection con)
        {
            string sql = $"INSERT INTO cards(card_id, name,damage,cat_id,username,package_id,element_id,mt_id) " +
                    $"VALUES(\'{id}\', \'{name}\', {damage}, {cat_id}, \'{username}\', {package_id}, {element_id}, {mt_id})";
            try
            {
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("QUERY: " + sql);
                }
            } 
            catch { Console.WriteLine($"ERROR while executing {sql}"); }
                
        }

        internal void CompleteCardInformation(NpgsqlConnection con)
        {
            //Only card_id available
            string sql = $"SELECT name, damage, cat_id, username, package_id, element_id, mt_id FROM cards WHERE card_id = \'{id}\'";
            using (var cmd = new NpgsqlCommand(sql, con))
            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            {
                Console.WriteLine("QUERY: " + sql);
                while (rdr.Read())
                {
                    name = rdr.GetString(0);
                    damage = rdr.GetDouble(1);
                    cat_id = rdr.GetInt32(2);
                    username = rdr.GetString(3);
                    package_id = rdr.GetInt32(4);
                    element_id = rdr.GetInt32(5);
                    mt_id = rdr.GetInt32(6);
                }
            }

            return;
        }

        internal bool CheckInDeck(Connector con)
        {
            List<string> deckCards = new List<string>();

            deckCards = con.SelectListString(4, $"SELECT card1, card2, card3, card4 FROM decks WHERE username = \'{username}\'");
            foreach( string card in deckCards)
            {
                if (card.Equals(id))
                    return true;
            }
            return false;
        }
    }
}
