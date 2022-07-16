using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoJwt
{
    public class AuThenticationRespone
    {
        public string JwtToken { get; set; }
    }
    public class JwtAuthenicationManager : IJwtAuthenicationManager
    {
        private string key;

        public JwtAuthenicationManager(string key)
        {
            this.key = key;
        }


        public AuThenticationRespone Autheniaction(string TaiKhoan, string MatKhau)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var toKenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, TaiKhoan)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new
                SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(toKenDescription);

            return new AuThenticationRespone
            {
                JwtToken = tokenHandler.WriteToken(token),
            };
        }
    }
}
