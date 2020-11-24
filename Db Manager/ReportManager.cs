using HouseholdUserApplication.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class ReportManager
    {
        
       public static Report GetReport(Dates dates,int id)
       {
            Report report = new Report();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.Build()))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT balances.*,billings.*,residents.firstname,residents.lastname,addresses.*  FROM billing_residents
                                        INNER JOIN residents 
                                        ON resident = residents.resident_id
                                        INNER JOIN residents_addresses as ra
                                        ON billing_residents.resident = ra.resident
                                        INNER JOIN billings 
                                        ON billings.address = ra.address
                                        INNER JOIN balances
                                        ON balances.balance_id = balance
                                        INNER JOIN addresses
                                        ON ra.address = address_id
                                        WHERE resident_id = @id
                                        AND year(open_date) = @year
                                        AND month(open_date) = @month

                                        ";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@year", dates.Years[0]);
                    cmd.Parameters.AddWithValue("@month", dates.Months[0]);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            report = new Report
                            {
                                Balance = new Balance
                                {
                                    Id = (int)reader["balance_id"],
                                    Utilities = (double)reader["utilities"],
                                    Services = (double)reader["services"],
                                    OpenDate = (DateTime)reader["open_date"],
                                    CloseDate = (DateTime)reader["close_date"]

                                },
                                Billing = new Billing
                                {
                                    Id= (int)reader["billing_id"],
                                    Payed = (double)reader["payed"],
                                    Remain = (double)reader["remain"],
                                    IsApproved = Convert.ToBoolean((sbyte)reader["is_approved"]),
                                    TotalBilling = (double)reader["total_billing"],
                                    Address = new Address
                                    {
                                        Id = (int)reader["address_id"],
                                        Street = (string)reader["street"],
                                        Number = (int)reader["number"]
                                        
                                    }
                                },
                                User = new User
                                {
                                    FirstName = reader["firstname"].ToString(),
                                    LastName = reader["lastname"].ToString()
                                }
                            };
                        }
                    }
                }
            }
            return report;
       }
    }
}
