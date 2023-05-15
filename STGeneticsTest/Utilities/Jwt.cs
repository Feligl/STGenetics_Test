using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace STGeneticsTest.Utilities
{
    public class Jwt
    {
        private readonly IConfiguration _configuration;
        public Jwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwt(bool Generate)
        {
            if (Generate)
            {
                var issuer = _configuration.GetSection("Jwt").GetValue<string>("Issuer");
                var audience = _configuration.GetSection("Jwt").GetValue<string>("Audience");
                var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt").GetValue<string>("Key"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    { new Claim("Id", Guid.NewGuid().ToString()), }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return stringToken;
            }
            return null;
        }

        #region Static Methods
        public static void JwtAuthenticationOptions(AuthenticationOptions arg)
        {
            arg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            arg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            arg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        public static void JwtValidationOptions(JwtBearerOptions arg, WebApplicationBuilder builder)
        {
            arg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        }
        #endregion
    }
}
