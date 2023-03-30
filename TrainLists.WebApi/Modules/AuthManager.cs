using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace TrainLists.WebApi.Modules
{
    public class AuthManager
    {
        public const string ISSUER = "L_Fabrichnaya";
        public const string AUDIENCE = "WebApi";
        const string KEY = "56f001515ec295e1-d5e58650dce4";
        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public static string GenerateToken(List<Claim> claims)
        { 
            JwtSecurityToken token = new(
                ISSUER,
                AUDIENCE,
                claims,
                expires : DateTime.Now.AddDays(LIFETIME),
                signingCredentials : new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}