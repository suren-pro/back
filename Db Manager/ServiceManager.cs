using HouseholdUserApplication.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class ServiceManager
    {
        public static List<Service> GetAvalibileServices(int id)
        {
            List<Service> services = new List<Service>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.Build()))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT  services.* FROM services
                                        INNER JOIN address_service as ad_s
                                        ON service_id = service
                                        INNER JOIN residents_addresses as ra
                                        ON ra.address = ad_s.address
                                        WHERE resident = @user";
                    cmd.Parameters.AddWithValue("@user", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Service service = new Service
                            {
                                Id = (int)reader["service_id"],
                                Name = reader["name"].ToString(),
                                Price = (double)reader["price"],
                                Type =(int)reader["type"]
                            };
                            services.Add(service);
                        }
                    }
                }
            }
            return services;
        }
    }
}
