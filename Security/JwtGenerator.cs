using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Security
{
    public class JwtGenerator
    {
        public static string GenerateJSONWebToken(int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            string host = "https://localhost:44388/";
            var claims = new List<Claim>()
            {
                new Claim("UserId",id.ToString())
            };
            var token = new JwtSecurityToken(
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              issuer:host,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
