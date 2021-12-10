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

        public GenerateJwt(IConfiguration config)
        {
            _config = config;
        }

        // Function to generate a JWT
        public string GenerateJWT(string type, string subject, User user)
        {
            // Define key and credentials
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims
            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("typ", type),
                new Claim("sub", subject),
                new Claim("role", user.Role)
            };

            // Define the token
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            // Writes the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // TODO implement validation of token
        public string ValidateToken(string token, string expectedType)
        {
            throw new NotImplementedException();
        }
    }
}
