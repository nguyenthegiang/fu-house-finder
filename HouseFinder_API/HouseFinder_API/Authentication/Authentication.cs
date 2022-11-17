using BusinessObjects;
using DataAccess.DTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HouseFinder_API.Authentication
{
    public class AuthenticationManager : IAuthentication
    {
        private readonly string key;

        public AuthenticationManager(string key)
        {
            this.key = key;
        }

        public string Authenticate(ResponseDTO user)
        {
            if (user == null)
                return null;
            string email;
            if (!String.IsNullOrWhiteSpace(user.Email))
            {
                email = user.Email;
            }
            else if (!String.IsNullOrWhiteSpace(user.FacebookUserId))
                email = user.FacebookUserId;
            else if (!String.IsNullOrWhiteSpace(user.GoogleUserId))
                email = user.GoogleUserId;
            else
                email = null;
            if (String.IsNullOrWhiteSpace(email))
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email,  email),
                    new Claim(ClaimTypes.Role, user.RoleName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = "HOUSEFINDER",
                Audience = "HOUSEFINDER"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
