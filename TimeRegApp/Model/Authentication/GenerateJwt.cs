using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimeReg_Api.TimeRegApp.Model.Account;

namespace TimeReg_Api.TimeRegApp.Model.Authentication
{
    public class GenerateJwt : IGenerateJwt
    {
        private IConfiguration _config;

        public GenerateJwt (IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ValidateToken(string token, string expectedType)
        {
            throw new NotImplementedException();
        }
    }
}
