using HouseholdUserApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Db_Manager
{
    public class EmailManager
    {
        public static void SendEmail(User user,string token)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential
                {
                    UserName = "surenbarseghyan00@gmail.com",
                    Password = "nssjqjqjwhnytnaa"
                }
            };

            MailAddress fromEmail = new MailAddress("surenbarseghyan00@gmail.com", "House resident reset password");


            using (MailMessage message = new MailMessage
            {
                From = fromEmail,
                Subject = $"Reseting password - {user.FirstName + " " + user.LastName}",
                IsBodyHtml = true,
                Body = $"Dear {user.FirstName + " " + user.LastName},<br> To reset your password click here <a href='https://localhost:44388?token={token}'>here</a>"
            })
            { 
                
                MailAddress toEmail = new MailAddress(user.Email, user.FirstName + " " + user.LastName);
                message.To.Add(toEmail);
                client.Send(message);
                

            }
        
        }
    }
}
