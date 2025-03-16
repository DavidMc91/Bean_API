
using Moq;
using Bean_API.Controllers;
using Bean_API.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Bean_API.Models;

namespace Bean_APITest.ControllerTests
{
    [TestClass]
    public class AuthControllerTests
    {
        public required Mock<ILogger<AuthController>> _mockLogger;
        public required Mock<IAuthService> _MockAuthService;
        public required AuthController _controller;

        [TestInitialize]
        public void Setup()
        {
            //Setup the objects and mock dependencies for the test methods
            _mockLogger = new Mock<ILogger<AuthController>>();
            _MockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockLogger.Object, _MockAuthService.Object);
        }

        [TestMethod]
        public void Login_ShouldReturnTokenAsString_WhenLoginDetailsAreValid()
        {
            //Arrange
            var login = new Login
            {
                Username = "user",
                Password = "password"
            };
            _MockAuthService.Setup(a => a.GenerateToken(login.Username)).Returns("TestToken");

            //Act
            var result = _controller.Login(login);

            //Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("{ Token = TestToken }", okResult.Value?.ToString());
            _MockAuthService.Verify(s => s.GenerateToken(It.IsAny<string>()), Times.Once); //Verify that the service method was called once
        }
    }
}
