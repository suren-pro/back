using HouseholdUserApplication.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class OptionManager
    {
        public static void AddOption(Option option)
        {
            option.Count = 0;
            int optionId, pollId;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@name", option.Name);
                    command.Parameters.AddWithValue("@count", option.Count);
                    command.CommandText = "INSERT INTO poll_options (name,count) " +
                        "VALUES(@name,@count)";
                    command.ExecuteNonQuery();

                }
                GetId(out optionId,out pollId,conn);
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@poll", pollId);
                    command.Parameters.AddWithValue("@option", optionId);
                    command.CommandText = "INSERT INTO poll_poll_options (poll_poll_id,poll_option_id)" +
                     "VALUES(@poll,@option)";
                    command.ExecuteNonQuery();
                }
            }
        }
        private static void  GetId(out int optionId, out int pollId, MySqlConnection conn)
        {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "getId";
                    command.Parameters.Add("opt", MySqlDbType.Int32).Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add("poll", MySqlDbType.Int32).Direction = ParameterDirection.InputOutput;
                    command.ExecuteNonQuery();

                    optionId = (int)command.Parameters["opt"].Value;
                    pollId = (int)command.Parameters["poll"].Value;

                }
        }

    }
}
