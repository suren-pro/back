using HouseholdUserApplication.CardUtils;
using HouseholdUserApplication.Models;
using HouseholdUserApplication.Security;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class UserManager
    {
        public static int UserExists(string username, string password)
        {
            int id;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {

                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "checkUser";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("login", username);
                cmd.Parameters.AddWithValue("pass", password);
                cmd.Parameters.Add("result", MySqlDbType.Int32).Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                try
                {
                    id = (int)cmd.Parameters["result"].Value;
                }
                catch
                {
                    return -1;
                }
            }

            return id;
        }
        public static User GetUserById(int id)
        {
            User user = new User();
            user.Address = new Address();
            
             using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
             {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT username, users.id,firstname,lastname,phone,email,street,number,block,is_free,latitude, longitude
                                                       FROM users
                                                       INNER JOIN addresses
                                                     ON users.address = address_id
                                                     WHERE users.id = @id ";
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = Convert.ToInt32(reader["Id"]);
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.Username = reader["Username"].ToString();
                            user.Phone = reader["Phone"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Address.Block = reader["block"].ToString();
                            user.Address.Number = (int)reader["number"];
                            user.Address.Street = reader["street"].ToString();
                        
                        }
                    }
                }
                
               
            }
                return user;
        }
        public static List<Card> SetCardColors(List<Card> cards,int id)
        {
            List<Card> cards1 = new List<Card>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM cards 
                                        WHERE user_id = @user_id";
                    cmd.Parameters.AddWithValue("@user_id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string bindingId = reader["binding_id"].ToString();
                            Card card = cards.SingleOrDefault(c => c.BindingId == bindingId);
                            card.Color = reader["color"].ToString();
                        }
                    }
                }
            }
            return cards;
        }
        public static int CheckLastOrder()
        {
            int id = 0;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM orders 
                                        ORDER BY id DESC
                                        LIMIT 1";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = (int)reader["id"]; 
                        }
                    }
                }
            }
            return id;
        }
        public static void AddOrder(string title)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO orders (title) VALUES(@title)";
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteCard(long cardNumber,int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"DELETE FROM cards 
                                        WHERE card_number = @card_number
                                        AND user = @userId";
                    cmd.Parameters.AddWithValue("@card_number", cardNumber);
                    cmd.Parameters.AddWithValue("@card_number", cardNumber);
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public static void Update(User user)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE users
                                        SET phone =@phone, username=@username, email = @email 
                                        WHERE id = @user_id";
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@phone", user.Phone);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@user_id", user.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdatePassword(string password, int id)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE users
                                        SET password = @password
                                        WHERE id = @user_id";
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@user_id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
