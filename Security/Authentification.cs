using HouseholdUserApplication.Db_Manager;
using HouseholdUserApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Security
{
    public class Authentification
    {
        public static User Login(Login login)
        {
            int id = UserManager.UserExists(login.Username, login.Password);
            if (id == -1)
                return null;

            return UserManager.GetUserById(id);

        }
        public static User UserLink(string email)
        {
            int id = UserManager.CheckUserByEmail(email);
            if (id == -1)
                return null;
            return UserManager.GetUserById(id);
        }
    }
}
