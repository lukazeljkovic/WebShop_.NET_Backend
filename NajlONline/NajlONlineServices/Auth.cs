using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NajlONline.Models;
using NajlONline.Services;
using NajlONlineServices.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineServices
{
    public class Auth : IJWTAuth
    {
        
        private string key;
        public IKorisnikService korisnik;
        public IConfiguration configuration;
        public Auth(IConfiguration configuration, IKorisnikService korisnik)
        {
            this.configuration = configuration;
            this.korisnik = korisnik;
        }
        public string Authentication(string username, string password)
        {


            key = configuration["JWT:Key"].ToString();

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role,korisnik.GetByKorisnickoIme(username).Uloga.Naziv)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }
}
