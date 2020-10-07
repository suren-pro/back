using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class ConnectionString
    {
        public static string Server { get; set; }
        public static uint Port { get; set; }
        public static string Database { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string Build()
        {
            return (new MySqlConnectionStringBuilder
            {
                Server = Server,
                Port = Port,
                Database = Database,
                UserID = User,
                Password = Password
            }).ToString();
        }
    }
}
