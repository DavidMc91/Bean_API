using Bean_API.Controllers;
using Bean_API.Dtos;
using Bean_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bean_APITest.ControllerTests
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
            _mockCoffeeBeanService.Setup(s => s.GetCoffeeBean_ByID_Async("66a374596122a40616cb8599"))
                .ReturnsAsync(fullCoffeeBean);

            //Act
            var result = await _controller.CreateCoffeeBean(createdCoffeeBean);

            //Assert
            var actionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(201, actionResult.StatusCode); //201 Created
            Assert.AreEqual("CreateCoffeeBean", actionResult.ActionName); //The name of the action we are referring to
            Assert.AreEqual(actionResult.Value, fullCoffeeBean); //The value returned by the action
            _mockCoffeeBeanService.Verify(s => s.CreateCoffeeBean_Async(It.IsAny<CreateCoffeeBeanDto>()), Times.Once); //Verify that the service method was called once
            _mockCoffeeBeanService.Verify(s => s.GetCoffeeBean_ByID_Async(It.IsAny<string>()), Times.Once); //Verify that the service method was called once
        }

        [TestMethod]
        public async Task GenerateBeanOfTheDay_ShouldReturnBeanOfTheDay_WhenServiceCreatesBotdSuccessfully()
        {
            //Arrange
            var responseCoffeeBean = new ResponseCoffeeBeanDto
            {
                Id = "66a374596122a40616cb8599",
                IndexNum = 0,
                IsBotd = true,
                Cost = 29.99m,
                Image = "https://www.example.com/image.jpg",
                ColourName = "Colour name",
                Name = "CoffeeBean of the day",
                Description = "This is a test coffee bean",
                CountryName = "Country name"
            };

            _mockCoffeeBeanService.Setup(s => s.GenerateBeanOfTheDay_Async())
                .ReturnsAsync(responseCoffeeBean);


            //Act
            var result = await _controller.GenerateBeanOfTheDay();

            //Assert
            var actionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(201, actionResult.StatusCode); //201 Created
            Assert.AreEqual("GenerateBeanOfTheDay", actionResult.ActionName); //The name of the action we are referring to
            Assert.AreEqual(actionResult.Value, responseCoffeeBean); //The value returned by the action
            _mockCoffeeBeanService.Verify(s => s.GenerateBeanOfTheDay_Async(), Times.Once); //Verify that the service method was called once
        }

        [TestMethod]
        public async Task GetCoffeeBean_All_Async_ShouldReturnCoffeeBeanList_WhenServiceGetsAllSuccessfully()
        {
            //Arrange
            var responseList = new List<ResponseCoffeeBeanDto>()
            {
                new ResponseCoffeeBeanDto
                {
                    Id = "66a374596122a40616cb8599",
                    IndexNum = 0,
                    IsBotd = true,
                    Cost = 29.99m,
                    Image = "https://www.example.com/image.jpg",
                    ColourName = "Colour name",
                    Name = "CoffeeBean 1",
                    Description = "This is a test coffee bean",
                    CountryName = "Country name"
                },
                new ResponseCoffeeBeanDto
                {
                    Id = "66a374596122a40616cb8555",
                    IndexNum = 1,
                    IsBotd = false,
                    Cost = 29.99m,
                    Image = "https://www.example.com/image.jpg",
                    ColourName = "Colour name",
                    Name = "CoffeeBean 2",
                    Description = "This is a test coffee bean",
                    CountryName = "Country name"
                }
            };

            _mockCoffeeBeanService.Setup(s => s.GetCoffeeBean_All_Async())
                .ReturnsAsync(responseList);

            //Act
            var result = await _controller.GetCoffeeBean_All_Async();

            //Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); //200 Ok
            var resultValue = okResult.Value as IEnumerable<ResponseCoffeeBeanDto>;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(2, resultValue.Count());
            Assert.AreEqual("CoffeeBean 1", resultValue.First().Name);
            _mockCoffeeBeanService.Verify(s => s.GetCoffeeBean_All_Async(), Times.Once); //Verify that the service method was called once
        }

        [TestMethod]
        public async Task GetCoffeeBean_ByID_Async_ShouldReturnCoffeeBean_WhenServiceFindsMatchSuccessfully()
        {
            //Arrange
            var responseCoffeeBean = new ResponseCoffeeBeanDto
            {
                Id = "66a374596122a40616cb8599",
                IndexNum = 0,
                IsBotd = true,
                Cost = 29.99m,
                Image = "https://www.example.com/image.jpg",
                ColourName = "Colour name",
                Name = "CoffeeBean 1",
                Description = "This is a test coffee bean",
                CountryName = "Country name"
            };

            _mockCoffeeBeanService.Setup(s => s.GetCoffeeBean_ByID_Async("66a374596122a40616cb8599"))
                .ReturnsAsync(responseCoffeeBean);

            //Act
            var result = await _controller.GetCoffeeBean_ByID_Async("66a374596122a40616cb8599");

            //Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); //200 Ok
            var resultValue = okResult.Value as ResponseCoffeeBeanDto;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual("CoffeeBean 1", resultValue.Name);
            _mockCoffeeBeanService.Verify(s => s.GetCoffeeBean_ByID_Async(It.IsAny<string>()), Times.Once); //Verify that the service method was called once
        }

        [TestMethod]
        public async Task GetCoffeeBean_BySearch_Async_ShouldReturnCoffeeBeanList_WhenServiceFindsMatchSuccessfully()
        {
            //Arrange
            var search = new SearchCoffeeBeanDto
            {
                Name = "DJM"
            };
            var responseList = new List<ResponseCoffeeBeanDto>()
            {
                new ResponseCoffeeBeanDto
                {
                    Id = "66a374596122a40616cb8599",
                    IndexNum = 0,
                    IsBotd = true,
                    Cost = 29.99m,
                    Image = "https://www.example.com/image.jpg",
                    ColourName = "Colour name",
                    Name = "CoffeeBean DJM 1",
                    Description = "This is a test coffee bean",
                    CountryName = "Country name"
                },
                new ResponseCoffeeBeanDto
                {
                    Id = "66a374596122a40616cb8555",
                    IndexNum = 1,
                    IsBotd = false,
                    Cost = 29.99m,
                    Image = "https://www.example.com/image.jpg",
                    ColourName = "Colour name",
                    Name = "CoffeeBean DJM 2",
                    Description = "This is a test coffee bean",
                    CountryName = "Country name"
                }
            };

            _mockCoffeeBeanService.Setup(s => s.GetCoffeeBean_BySearch_Async(search))
                .ReturnsAsync(responseList);

            //Act
            var result = await _controller.GetCoffeeBean_BySearch_Async(search);

            //Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); //200 Ok
            var resultValue = okResult.Value as IEnumerable<ResponseCoffeeBeanDto>;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(2, resultValue.Count());
            Assert.AreEqual("CoffeeBean DJM 1", resultValue.First().Name);
            _mockCoffeeBeanService.Verify(s => s.GetCoffeeBean_BySearch_Async(It.IsAny<SearchCoffeeBeanDto>()), Times.Once); //Verify that the service method was called once
        }

        [TestMethod]
        public async Task UpdateCoffeeBean_Async_ShouldReturnUpdatedCoffeeBean_WhenServiceUpdatesRecordSuccessfully()
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
                Name = "Updated Coffee Bean",
                Description = "This is a test coffee bean",
                CountryId = 1
            };

            _mockCoffeeBeanService.Setup(s => s.UpdateCoffeeBean_ByID_Async("66a374596122a40616cb8599", createdCoffeeBean))
                .ReturnsAsync(createdCoffeeBean);

            //Act
            var result = await _controller.UpdateCoffeeBean_Async("66a374596122a40616cb8599", createdCoffeeBean);

            //Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); //200 Ok
            var resultValue = okResult.Value as CreateCoffeeBeanDto;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual("Updated Coffee Bean", resultValue.Name);
            _mockCoffeeBeanService.Verify(s => s.UpdateCoffeeBean_ByID_Async(It.IsAny<string>(), It.IsAny<CreateCoffeeBeanDto>()), Times.Once); //Verify that the service method was called once
        }

        [TestMethod]
        public async Task DeleteCoffeeBean_ByID_Async_ShouldReturn200Response_WhenServicedeletesRecordSuccessfully()
        {
            //Arrange
            var id = "66a374596122a40616cb8599";

            _mockCoffeeBeanService.Setup(s => s.DeleteCoffeeBean_ByID_Async(id))
                .ReturnsAsync(true);

            //Act
            var result = await _controller.DeleteCoffeeBean_Async(id);

            //Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); //200 Ok
            Assert.AreEqual("Deleted successfully", okResult.Value);
            _mockCoffeeBeanService.Verify(s => s.DeleteCoffeeBean_ByID_Async(It.IsAny<string>()), Times.Once); //Verify that the service method was called once
        }

    }
}
