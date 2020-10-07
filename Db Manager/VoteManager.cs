using HouseholdUserApplication.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class VoteManager
    {
        public static void Vote(Vote vote) 
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using(MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"INSERT INTO votes (user_id,option_id)
                                            VALUES(@user_id,@option_id) ";
                    command.Parameters.AddWithValue("@user_id", vote.UserId);
                    command.Parameters.AddWithValue("@option_id", vote.Option.Id);
                    command.ExecuteNonQuery();
                }
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"UPDATE  poll_options
                                            SET count =@count
                                            WHERE option_id = @option_id";
                    command.Parameters.AddWithValue("@option_id", vote.Option.Id);
                    command.Parameters.AddWithValue("@count", vote.Option.Count);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
