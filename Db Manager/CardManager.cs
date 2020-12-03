using HouseholdUserApplication.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class CardManager
    {
        public static void AddCard(string bindingId)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = "INSERT INTO cards(binding_id)" +
                        "VALUES(@bindingId)";
                    command.Parameters.AddWithValue("@bindingId", bindingId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateCard(List<CardChange> cards)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                foreach (var card in cards)
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = @"UPDATE cards 
                                            SET name = @name
                                            WHERE binding_id= @binding_id";
                        command.Parameters.AddWithValue("@bindingId", card.BindingId);
                        command.Parameters.AddWithValue("@name", card.Name);
                        command.ExecuteNonQuery();
                    }
                }
               
            }
        }
    }
}
