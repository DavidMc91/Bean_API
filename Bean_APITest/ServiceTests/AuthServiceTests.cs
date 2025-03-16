using Bean_API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bean_APITest.ServiceTests
{
    [TestClass]
    public class AuthServiceTests
    {
        public required Mock<ILogger<AuthService>> _mockLogger;
        public required Mock<IConfiguration> _mockConfiguration;
        public required AuthService _authService;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<AuthService>>();

            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "This1Is2A3Test4Key5For6Testing7!" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" }
            };

            _mockConfiguration = new Mock<IConfiguration>();
            foreach (var setting in inMemorySettings)
            {
                _mockConfiguration.Setup(c => c[setting.Key]).Returns(setting.Value);
            }

            _authService = new AuthService(_mockLogger.Object, _mockConfiguration.Object);
        }


        [TestMethod]
        public void GenerateToken_ShouldReturnValidToken_WhenValidUsernameIsGiven()
        {
            //Arrange
            var username = "TestUser";

            //Act
            var token = _authService.GenerateToken(username);

            //Assert
            Assert.IsNotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.AreEqual("TestIssuer", jwtToken.Issuer);
            Assert.AreEqual("TestAudience", jwtToken.Audiences.First());
            Assert.AreEqual(username, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.IsTrue(jwtToken.ValidTo > DateTime.UtcNow); //Ensure expiration is set correctly
        }
    }
}