using HouseholdUserApplication.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class NewsManager
    {
        public static void  AddNews(News news)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                news.Date = DateTime.Now;
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@title", news.Title);
                    cmd.Parameters.AddWithValue("@description",news.Description);
                    cmd.Parameters.AddWithValue("@image", news.Image);
                    cmd.Parameters.AddWithValue("@date", news.Date);
                    cmd.CommandText = $"INSERT INTO news(title,description,image,date)" +
                    $" VALUES(@title,@description,@image,@date)";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<News> GetNews()
        {
            List<News> newsList = new List<News>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT * FROM news";
                    cmd.Connection = conn;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            News news = new News()
                            {
                                Id = (int)reader["id"],
                                Title = reader["title"].ToString(),
                                Description = reader["description"].ToString(),
                                Image = reader["image"] is DBNull ? null : Convert.ToString(reader["image"]),
                                Date = (DateTime)reader["date"],
                            };
                            newsList.Add(news);
                        }
                    }
                }
            }
            return newsList;
        }
    }
}
