﻿using HouseholdUserApplication.Models;
using MySql.Data.MySqlClient;
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
                   ON address = a.address_id";
                    cmd.Connection = conn;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Activity activity = new Activity();

                            activity.Id = (int)reader["id"];
                            activity.ServiceName = reader["service_name"].ToString();
                            activity.CardNumber = Convert.ToDouble(reader["card_number"]);
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
                            activityList.Add(activity);
                        }
                    }
                }
            }
            return activityList;
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
                    WHERE activities.id = @activity_id";
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@activity_id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           

                            activity.Id = (int)reader["id"];
                            activity.ServiceName = reader["service_name"].ToString();
                            activity.CardNumber = Convert.ToDouble(reader["card_number"]);
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
                                        INNER JOIN users as u
                                        ON u.address = address_id
                                        WHERE year(date) = @yaer
                                        AND u.id = @user_id
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
                            activity.CardNumber = Convert.ToDouble(reader["card_number"]);
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
                            activities.Add(activity);

                        }
                    }
                }
            }
            return activities;
        }
    }
}

