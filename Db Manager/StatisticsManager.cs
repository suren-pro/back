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
    public class StatisticsManager
    {
        public static List<Point> GetPriceHistory(Dates dates,int userId)
        {
            List<Point> points = new List<Point>();
            List<UtilityRecord> utilityRecords = new List<UtilityRecord>();
            DateTime date1 = new DateTime(dates.Years[0],dates.Months[0],1);
            DateTime date2 = new DateTime(dates.Years[1], dates.Months[1], 1);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT price,value,date FROM utility_records as UR
                                        INNER JOIN utilities 
                                        ON utility = utility_id
                                        INNER JOIN residents_addresses AS U
                                        ON UR.address = U.address
                                         WHERE U.resident = @user_id
                                        AND name=@name
                                         AND 
                                        (YEAR(`date`)=@year1 AND MONTH(`date`) BETWEEN @month1 AND @month2) OR 
										(YEAR(`date`)=@year2 AND MONTH(`date`) BETWEEN @month1 AND @month2)
                                        ORDER BY date
                                        ";
                    cmd.Parameters.AddWithValue(@"year1", date1.Year);
                    cmd.Parameters.AddWithValue(@"year2", date2.Year);
                    cmd.Parameters.AddWithValue("@month1", date1.Month);
                    cmd.Parameters.AddWithValue("@month2", date2.Month);
                    cmd.Parameters.AddWithValue(@"user_id", userId);
                    cmd.Parameters.AddWithValue(@"name", dates.Name);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            UtilityRecord utilityRecord = new UtilityRecord
                            {
                                
                                Value = (int)reader["value"],
                                Date = Convert.ToDateTime(reader["date"])

                            };
                            Utility utility = new Utility
                            {
                                Price = Convert.ToDouble(reader["price"])
                            };
                            utilityRecord.Utility = utility;

                            utilityRecords.Add(utilityRecord);
                        } 
                    }

                    
                }
                for (int i = 0; i < utilityRecords.Count-1; i++)
                {
                    double price = utilityRecords[i].Utility.Price;
                    int currentRecord = utilityRecords[i].Value;
                    int nextRecord = utilityRecords[i + 1].Value;
                    
                    Point point = new Point
                    {
                        X = utilityRecords[i].Date.ToString(),
                        Y =  Convert.ToInt32((nextRecord - currentRecord) * price)
                    };
                    points.Add(point);

                }
            }
            return points;
        }
        public static List<Point> GetUtilityRecordsByDate(Dates dates,int userId) 
        {
            List<Point> points = new List<Point>();
            User user = UserManager.GetUserById(userId);
            DateTime date1 = new DateTime(dates.Years[0], dates.Months[0], 28);
            DateTime date2 = new DateTime(dates.Years[1], dates.Months[1], 28);
           
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT utility_record_id,date,value,U.address
                                        FROM utility_records AS UR 
                                        INNER JOIN residents_addresses AS U
                                        ON UR.address = U.address
                                        INNER JOIN utilities 
                                        ON utility = utility_id
                                        WHERE U.resident = @user_id
                                        AND name = @name
                                        AND 
                                        (YEAR(`date`)=@year1 AND MONTH(`date`) BETWEEN @month1 AND @month2) OR 
										(YEAR(`date`)=@year2 AND MONTH(`date`) BETWEEN @month1 AND @month2)
                                        ORDER BY date
                                        ";
                    cmd.Parameters.AddWithValue(@"year1", date1.Year);
                    cmd.Parameters.AddWithValue(@"year2", date2.Year);
                    cmd.Parameters.AddWithValue("@month1", date1.Month);
                    cmd.Parameters.AddWithValue("@month2", date2.Month);
                    cmd.Parameters.AddWithValue(@"user_id",userId);
                    cmd.Parameters.AddWithValue(@"name", dates.Name);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        { 
                           
                            points.Add(new Point
                            {
                                X = reader["date"].ToString(),
                            Y = (int)reader["value"]
                            }); 
                        }
                    }
                }
            }
          
            return points;
        }
        public static Total GetTotal(Dates dates,int userId)
        {
            Total total = new Total();
            total.Utilities = new List<List<object>>();
            List<UtilityRecord> utilityRecords = new List<UtilityRecord>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection =conn;
                    cmd.CommandText = @"SELECT utility_record_id,date,value,U.address,name,price
                                        FROM utility_records AS UR 
                                        INNER JOIN residents_addresses AS U
                                         ON UR.address = U.address
                                        INNER JOIN utilities 
                                        ON utility = utility_id
                                        WHERE U.resident = @userId
                                        AND year(date) = @year1
                                         ORDER BY utility,date
                                        ";
                    cmd.Parameters.AddWithValue("@year1",(Convert.ToInt32(dates.Years[0])));
                    cmd.Parameters.AddWithValue("@month1",(Convert.ToInt32(dates.Months[0])));
                    cmd.Parameters.AddWithValue("@month2",(Convert.ToInt32(dates.Months[0])+1));

                    cmd.Parameters.AddWithValue("@userId", (Convert.ToInt32(userId)));
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UtilityRecord utilityRecord = new UtilityRecord
                            {
                                Value = (int)reader["value"],
                                Date = Convert.ToDateTime(reader["date"])
                            };
                            Utility utility = new Utility
                            {
                                Name = reader["name"].ToString(),
                                Price = (double)reader["price"],

                            };
                            utilityRecord.Utility = utility;
                            utilityRecords.Add(utilityRecord);
                        }
                    }

                }

                for (int i = 0; i < utilityRecords.Count-1; i++)
                {
                    if (utilityRecords[i].Utility.Name == utilityRecords[i + 1].Utility.Name)
                    {
                        double price = utilityRecords[i].Utility.Price;
                        int current = utilityRecords[i].Value;
                        int next = utilityRecords[i + 1].Value;

                        UtilityDetail utilityDetail = new UtilityDetail
                        {
                            Name = utilityRecords[i].Utility.Name,
                            Price = Convert.ToDouble((next - current) * price),
                            Date = utilityRecords[i].Date
                        };
                        if (utilityDetail.Date.Month == Convert.ToInt32(dates.Months[0]))
                        {
                            total.Utilities.Add(new List<object>() { utilityDetail.Name, utilityDetail.Price });

                        }
                    }
                }
                

            }
            total.Services = new Dictionary<string, int>
            {
                {"Nanny",500 },
                { "Cleaning",600 }
            };
            return total;
        }
        public static Total GetTotalGraph(int userId)
        {
            Total total = new Total();
            total.Utilities = new List<List<object>>();
            List<string> names = new List<string>();
            List<UtilityRecord> utilityRecords = new List<UtilityRecord>();
            List<UtilityDetail> utilityDetails = new List<UtilityDetail>();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString.Build()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT name FROM utilities";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            total.Utilities.Add(new List<object>() { reader["name"].ToString() });
                        }
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT utility_record_id,date,value,U.address,name,price
                                        FROM utility_records AS UR 
                                        INNER JOIN residents_addresses AS U
                                         ON UR.address = U.address
                                        INNER JOIN utilities 
                                        ON utility = utility_id
                                        WHERE U.resident = @userId
                                        AND year(date) = year(curdate())
                                         ORDER BY utility,date";
                    cmd.Parameters.AddWithValue("@userId", (Convert.ToInt32(userId)));
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UtilityRecord utilityRecord = new UtilityRecord
                            {
                                Value = (int)reader["value"],
                                Date = Convert.ToDateTime(reader["date"])
                            };
                            Utility utility = new Utility
                            {
                                Name = reader["name"].ToString(),
                                Price = (double)reader["price"],

                            };
                            utilityRecord.Utility = utility;
                            utilityRecords.Add(utilityRecord);
                        }
                    }
                }
                int next = 0, current = 0;
                
                for (int i = 0; i < utilityRecords.Count-1; i++)
                {
                    

                    if(utilityRecords[i].Utility.Name == utilityRecords[i + 1].Utility.Name)
                    {
                     double price = utilityRecords[i].Utility.Price;
                     current = utilityRecords[i].Value;
                     next = utilityRecords[i +1].Value;
                        UtilityDetail utilityDetail = new UtilityDetail
                        {
                            Name = utilityRecords[i].Utility.Name,
                            Price = Convert.ToDouble((next-current) * price),
                            Date = utilityRecords[i].Date
                        };
                        utilityDetails.Add(utilityDetail);

                    }
                }
                foreach (List<object> utilities in total.Utilities)
                {
                    foreach (var utilityDetail in utilityDetails)
                    {
                        if(utilityDetail.Name == utilities[0].ToString())
                        utilities.Add(utilityDetail.Price);
                    }
                }

            }
            total.Services = new Dictionary<string, int>
            {
                {"Nanny",500 },
                { "Cleaning",600}
            };
            return total;
        }
        public static List<UtilityDetail> GetUtilityDetails(Dates dates,int id)
        {
            List<UtilityRecord> utilityRecords = new List<UtilityRecord>();
            List<UtilityDetail> utilityDetails = new List<UtilityDetail>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.Build()))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT price,value,date,name FROM utility_records as UR
                                        INNER JOIN utilities 
                                        ON utility = utility_id
                                        INNER JOIN residents_addresses AS U
                                        ON UR.address = U.address
                                         WHERE U.resident = @id
                                         AND 
                                        (YEAR(`date`)=@year AND MONTH(`date`) BETWEEN @month-1 AND @month) OR 
										(YEAR(`date`)=@year AND MONTH(`date`) BETWEEN @month-1 AND @month)
                                        ORDER BY name";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@year", dates.Years[0]);
                    cmd.Parameters.AddWithValue("@month", dates.Months[0]);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UtilityRecord utilityRecord = new UtilityRecord
                            {
                                Value = (int)reader["value"],
                                Date = Convert.ToDateTime(reader["date"])
                            };
                            Utility utility = new Utility
                            {
                                Name = reader["name"].ToString(),
                                Price = (double)reader["price"],

                            };
                            utilityRecord.Utility = utility;
                            utilityRecords.Add(utilityRecord);
                        }
                    }
                }
                int next = 0, current = 0;

                for (int i = 0; i < utilityRecords.Count - 1; i++)
                {


                    if (utilityRecords[i].Utility.Name == utilityRecords[i + 1].Utility.Name)
                    {
                        double price = utilityRecords[i].Utility.Price;
                        current = utilityRecords[i].Value;
                        next = utilityRecords[i + 1].Value;
                        UtilityDetail utilityDetail = new UtilityDetail
                        {
                            Name = utilityRecords[i].Utility.Name,
                            Price = Convert.ToDouble((next - current) * price),
                            Date = utilityRecords[i].Date
                        };
                        utilityDetails.Add(utilityDetail);

                    }
                }
            }
            return utilityDetails;
            }
        }


    }

