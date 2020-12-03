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
                    cmd.CommandText = @"SELECT * FROM billing_residents
                                        INNER JOIN residents as r
                                        ON resident = resident_id
                                        INNER JOIN residents_addresses as ra 
                                        ON resident_id = ra.resident
                                        INNER JOIN addresses
                                        ON address_id = address
                                        WHERE resident_id = @id";
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = Convert.ToInt32(reader["resident_id"]);
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.Username = reader["Username"].ToString();
                            user.Phone = reader["Phone_number"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Address.Id = (int)reader["address_id"];
                            user.Address.Block = reader["block"].ToString();
                            user.Address.Street = reader["street"].ToString();
                            user.Address.Number = (int)reader["number"];
                            user.QrCode = reader["qr_code"].ToString();
                        }
                    }
                }
                
               
            }
                return user;
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
        public static int CheckUserByEmail(string email)
        {
            int id;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {

                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "checkUserByEmail";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("e", email);
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
        public static void AddOrder(string title)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO orders (orderNumber) VALUES(@orderNumber)";
                    cmd.Parameters.AddWithValue("@orderNumber", title);
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
                    cmd.CommandText = @"UPDATE residents
                                        SET phone_number =@phone,email = @email 
                                        WHERE resident_id = @user_id";
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@phone", user.Phone);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@user_id", user.Id);
                    cmd.ExecuteNonQuery();
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE billing_residents
                                        SET username =@username
                                        WHERE resident = @user_id";
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
                    cmd.CommandText = @"UPDATE billing_residents
                                        SET password = @password
                                        WHERE resident = @user_id";
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@user_id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
