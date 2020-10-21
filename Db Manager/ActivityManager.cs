using HouseholdUserApplication.Models;
using MySql.Data.MySqlClient;
using Renci.SshNet.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class ActivityManager
    {
        public static List<Activity> GetActivity()
        {
            List<Activity> activityList = new List<Activity>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM activities
                                     INNER JOIN addresses AS a
                                     ON address = a.address_id
                                     INNER JOIN residents_addresses as ra
                                     ON a.address_id =  ra.address
                                     ";
                    cmd.Connection = conn;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Activity activity = new Activity();

                            activity.Id = (int)reader["id"];
                            activity.ServiceName = reader["service_name"].ToString();
                            activity.CardNumber = reader["card_number"].ToString();
                            activity.Fee = Convert.ToDouble(reader["fee"]);
                            activity.Total = Convert.ToDouble(reader["total"]);
                            activity.OrderId = reader["order_id"].ToString();
                            activity.Rrn = reader["rrn"].ToString();
                            activity.Customer = reader["customer"].ToString();
                            activity.Description = reader["description"].ToString();
                            activity.CustomerId = (int)reader["customer_id"]; 
                            activity.Amount = Convert.ToDouble(reader["amount"]);
                            activity.Date = Convert.ToDateTime(reader["date"]);
                               
                               
                            
                            Address address = new Address
                            {
                                Block = reader["block"].ToString(),
                                Number = (int)reader["number"],
                                Street = reader["street"].ToString(),
                                Id = (int)reader["address"],
                            };
                            activity.Address = address;
                            activityList.Add(activity);
                        }
                    }
                }
            }
            return activityList;
        }

        public static void AddActivity(OrderStatusModel orderStatus,int id)
        {
            Activity activity = new Activity
            {
                ServiceName ="Card2Card",
                Amount = Convert.ToDouble(orderStatus.Amount)/100,
                Fee = 0,
                Rrn = orderStatus.AuthRefNum,
                Date = Convert.ToDateTime(orderStatus.Date),
                CardNumber =orderStatus.CardAuthInfo.Pan,
                OrderId = orderStatus.OrderNumber,
                Customer = orderStatus.CardAuthInfo.CardHolderName,
                Description ="asd",
               

            };
            User user = UserManager.GetUserById(id);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO activities (service_name,amount,order_id,fee,total,rrn,customer_id,customer,address,description,card_number,date)
                                        VALUES(@service_name,@amount,@order_id,@fee,@total,@rrn,@customer_id,@customer,@address,@description,@card_number,@date)";
                    cmd.Parameters.AddWithValue("@service_name", activity.ServiceName);
                    cmd.Parameters.AddWithValue("@amount", activity.Amount);
                    cmd.Parameters.AddWithValue("order_id", activity.OrderId);
                    cmd.Parameters.AddWithValue("@fee", activity.Fee);
                    cmd.Parameters.AddWithValue("@total", activity.Total);
                    cmd.Parameters.AddWithValue("@rrn", activity.Rrn);
                    cmd.Parameters.AddWithValue("@card_number", activity.CardNumber);
                    cmd.Parameters.AddWithValue("@customer_id", user.Id);
                    cmd.Parameters.AddWithValue("@customer", activity.Customer);
                    cmd.Parameters.AddWithValue("@address", user.Address.Id);
                    cmd.Parameters.AddWithValue("@description", activity.Description);
                    cmd.Parameters.AddWithValue("@date", activity.Date);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static Activity GetActivity(int id)
        {
            Activity activity = new Activity();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM activities
                    INNER JOIN addresses AS a
                   ON address = a.address_id
                    INNER J
                    WHERE activities.id = @activity_id";
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@activity_id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           

                            activity.Id = (int)reader["id"];
                            activity.ServiceName = reader["service_name"].ToString();
                            activity.CardNumber = reader["card_number"].ToString();
                            activity.Fee = Convert.ToDouble(reader["fee"]);
                            activity.Total = Convert.ToDouble(reader["total"]);
                            activity.OrderId = reader["order_id"].ToString();
                            activity.Rrn = reader["rrn"].ToString();
                            activity.Customer = reader["customer"].ToString();
                            activity.Description = reader["description"].ToString();
                            activity.Amount = Convert.ToDouble(reader["amount"]);
                            activity.Date = Convert.ToDateTime(reader["date"]);



                            Address address = new Address
                            {
                                Block = reader["block"].ToString(),
                                Number = (int)reader["number"],
                                Street = reader["street"].ToString(),
                                Id = (int)reader["address"],
                            };
                            activity.Address = address;
                        
                        }
                    }
                }
            }
            return activity;
        }
        public static string DownloadActivity(Activity activity)
        {

            return "";
        }
        public static Billing GetBilling(int id) 
        {
            Billing billing =new Billing();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM billings as bi
                                        INNER JOIN balances as b
                                        ON balance = b.balance_id
                                        INNER JOIN residents_addresses as ra
                                        ON bi.address = ra.address
                                        WHERE resident = @resident";
                    cmd.Parameters.AddWithValue("@resident", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            billing.Payed = (double)reader["payed"];
                            billing.Remain = (double)reader["remain"];
                            billing.TotalBilling =(double)reader["total_billing"];

                        }
                    }
                    return billing;
                }
            }
        }
       
        public static List<Activity> GetActivitiesByDate(Dates dates,int id)
        {
            List<Activity> activities = new List<Activity>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT A.*,Ad.* FROM activities as A
                                        INNER JOIN addresses as Ad
                                        ON address_id = A.address
                                        INNER JOIN residents_addresses as u
                                        ON u.address = address_id
                                        WHERE year(date) = @yaer
                                        AND u.resident = @user_id
                                        AND month(date) = @month";
                                        
                    cmd.Parameters.AddWithValue("@yaer",dates.Years[0]);
                    cmd.Parameters.AddWithValue("@month",dates.Months[0]);
                    cmd.Parameters.AddWithValue("@user_id",id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Activity activity = new Activity();


                            activity.Id = (int)reader["id"];
                            activity.ServiceName = reader["service_name"].ToString();
                            activity.CardNumber = reader["card_number"].ToString();
                            activity.Fee = Convert.ToDouble(reader["fee"]);
                            activity.Total = Convert.ToDouble(reader["total"]);
                            activity.OrderId = reader["order_id"].ToString();
                            activity.Rrn = reader["rrn"].ToString();
                            activity.Customer = reader["customer"].ToString();
                            activity.CustomerId = (int)reader["customer_id"];
                            activity.Description = reader["description"].ToString();
                            activity.Amount = Convert.ToDouble(reader["amount"]);
                            activity.Date = Convert.ToDateTime(reader["date"]);
                            Address address = new Address
                            {
                                Block = reader["block"].ToString(),
                                Number = (int)reader["number"],
                                Street = reader["street"].ToString(),
                                Id = (int)reader["address"],
                            };
                            activity.Address = address;
                            activities.Add(activity);

                        }
                    }
                }
            }
            return activities;
        }
    }
}

