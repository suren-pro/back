using HouseholdUserApplication.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    interface ICollection
    {

    }
    public class PollManager
    {
        public static void AddPoll(PollOption poll)
        {

            poll.Poll.Date = DateTime.Now;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = "INSERT INTO polls(title,description,date)" +
                        "VALUES(@title,@description,@datetime)";
                    command.Parameters.AddWithValue("@title", poll.Poll.Title);
                    command.Parameters.AddWithValue("@description", poll.Poll.Description);
                    command.Parameters.AddWithValue("@datetime", poll.Poll.Date);
                    command.ExecuteNonQuery();
                }
                foreach (var option in poll.Options)
                {
                    OptionManager.AddOption(option);
                }
            }
        }

        public static PollOption GetPoll(int id)
        {
            PollOption poll = new PollOption();
            poll.Options = new List<Option>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM polls
                                        INNER JOIN poll_poll_options
                                        ON poll_id = poll_poll_id";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            poll.Id = id;
                            poll.Poll = new Poll();
                            poll.Poll.Id = Convert.ToInt32(reader["poll_id"]);
                            poll.Poll.Title = reader["title"].ToString();
                            poll.Poll.Description = reader["description"].ToString();
                            poll.Poll.Date = Convert.ToDateTime(reader["date"]);
                        }
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@id", poll.Poll.Id);
                    cmd.CommandText = @"SELECT * FROM poll_options 
                                       INNER JOIN poll_poll_options  
                                       ON option_id = poll_option_id 
                                       WHERE poll_poll_id = @id";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Option option = new Option()
                            {
                                Id = Convert.ToInt32(reader["poll_option_id"]),
                                Name = reader["name"].ToString(),
                                Count = Convert.ToInt32(reader["count"])
                            };
                            poll.Options.Add(option);
                        }
                    }
                }

            }
            return poll;
        }
        public static List<PollOption> GetPolls(int userId)
        {
            List<PollOption> pollOptions = new List<PollOption>();
            List<Option> options = new List<Option>();

            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM polls ";
                                      
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PollOption pollOption = new PollOption
                            {
                                Id = Convert.ToInt32(reader["poll_id"]),
                                Poll = new Poll()
                                {
                                    Id = Convert.ToInt32(reader["poll_id"]),
                                    Title = reader["title"].ToString(),
                                    Description = reader["description"].ToString(),
                                },
                                Options = new List<Option>(),
                                Selected = new VoteCheck()
                                {
                                    OptionId = 0
                                }
                                
                            };
                            pollOptions.Add(pollOption);
                        }
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT poll_poll_id,poll_option_id,name,count FROM poll_poll_options 
                                        RIGHT JOIN  poll_options
                                        ON option_id = poll_option_id";
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["poll_poll_id"]);
                            Option option = new Option
                            {
                                Id = Convert.ToInt32(reader["poll_option_id"]),
                                Name = reader["name"].ToString(),
                                Count = Convert.ToInt32(reader["count"])
                            };
                            var pollOption = pollOptions.Where(i => i.Id == id).SingleOrDefault();
                            pollOption.Options.Add(option);
                        }
                    }

                }
               
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT poll_poll_id,poll_option_id,user_id,name,count FROM poll_poll_options AS ppo
                                                LEFT JOIN votes ON poll_option_id = option_id
                                                INNER JOIN  poll_options  AS o
                                                ON o.option_id = ppo.poll_option_id
                                                WHERE user_id = @user_id";
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["poll_poll_id"]);
                            int userIdFromDB = Convert.ToInt32(reader["user_id"]);
                            var pollOption = pollOptions.Where(i => i.Id == id).SingleOrDefault();
                            Vote vote = new Vote();
                            if (userId == userIdFromDB)
                            {
                                pollOption.Selected.Selected = true;
                                pollOption.Selected.OptionId = Convert.ToInt32(reader["poll_option_id"]);
                            }
                        }
                    }
                }
            }
            return pollOptions;
        }
    }
}
