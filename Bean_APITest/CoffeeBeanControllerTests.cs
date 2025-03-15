using Bean_API.Controllers;
using Bean_API.Dtos;
using Bean_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bean_APITest
{
    [TestClass]
    public class CoffeeBeanControllerTests
    {
        public required Mock<ILogger<CoffeeBeanController>> _mockLogger;
        public required Mock<ICoffeeBeanService> _mockCoffeeBeanService;
        public required CoffeeBeanController _controller;

        [TestInitialize]
        public void Setup()
        {
            //Setup the objects and mock dependencies for the test methods
            _mockLogger = new Mock<ILogger<CoffeeBeanController>>();
            _mockCoffeeBeanService = new Mock<ICoffeeBeanService>();
            _controller = new CoffeeBeanController(_mockLogger.Object, _mockCoffeeBeanService.Object);
        }


        [TestMethod]
        public async Task CreateCoffeeBean_ShouldReturnCreatedResult_WhenServiceCreatesCoffeeBeanSuccessfully()
        {
            //Arrange
            var createdCoffeeBean = new CreateCoffeeBeanDto()
            {
                Id = "66a374596122a40616cb8599",
                IndexNum = 0,
                IsBotd = 0,
                Cost = 29.99m,
                Image = "https://www.example.com/image.jpg",
                ColourId = 1,
                Name = "Test Coffee Bean",
                Description = "This is a test coffee bean",
                CountryId = 1
            };
            var fullCoffeeBean = new ResponseCoffeeBeanDto
            {
                Id = "66a374596122a40616cb8599",
                IndexNum = 0,
                IsBotd = false,
                Cost = 29.99m,
                Image = "https://www.example.com/image.jpg",
                ColourName = "Colour name",
                Name = "Test Coffee Bean",
                Description = "This is a test coffee bean",
                CountryName = "Country name"
            };

            //...mocking the service methods that will be used in the "CreateCoffeeBean" controller method
            _mockCoffeeBeanService.Setup(s => s.CreateCoffeeBean_Async(createdCoffeeBean))
                .ReturnsAsync(createdCoffeeBean);
            _mockCoffeeBeanService.Setup(s => s.GetCoffeeBean_ByID_Async("1"))
                .ReturnsAsync(fullCoffeeBean);

            //Act
            var result = await _controller.CreateCoffeeBean(createdCoffeeBean);

            //Assert
            var actionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(201, actionResult.StatusCode); //201 Created
            Assert.AreEqual("CreateCoffeeBean", actionResult.ActionName); //The name of the action we are referring to
            _mockCoffeeBeanService.Verify(s => s.CreateCoffeeBean_Async(It.IsAny<CreateCoffeeBeanDto>()), Times.Once); //Verify that the service method was called once
            _mockCoffeeBeanService.Verify(s => s.GetCoffeeBean_ByID_Async(It.IsAny<string>()), Times.Once); //Verify that the service method was called once
        }
    }
}
