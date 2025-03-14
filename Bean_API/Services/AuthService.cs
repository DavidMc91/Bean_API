using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Bean_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<CoffeeBeanService> _logger;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthService(ILogger<CoffeeBeanService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Key is not configured");
            _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Issuer is not configured");
            _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Audience is not configured");
        }

        public string GenerateToken(string username)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }
        }
    }
}
