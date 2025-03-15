using Bean_API.Dtos;
using Bean_API.Models;
using Bean_API.Repository;
using Bean_API.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bean_APITest
{
    [TestClass]
    public class CoffeeBeanServiceTests
    {
        public required Mock<ILogger<CoffeeBeanService>> _mockLogger;
        public required Mock<ICoffeeBeanRepository> _mockCoffeeBeanRepository;
        public required ICoffeeBeanService _coffeeBeanService;
        
        [TestInitialize]
        public void Setup()
        {
            //Setup the objects and mock dependencies for the test methods
            _mockCoffeeBeanRepository = new Mock<ICoffeeBeanRepository>();
            _mockLogger = new Mock<ILogger<CoffeeBeanService>>();
            _coffeeBeanService = new CoffeeBeanService(_mockLogger.Object, _mockCoffeeBeanRepository.Object);
        }

        [TestMethod]
        public async Task CreateCoffeeBean_Async_ShouldCreateCoffeeBean_WhenValidDtoIsGiven()
        {
            //Arrange
            var coffeeBeanDto = new CreateCoffeeBeanDto
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

            _mockCoffeeBeanRepository.Setup(r => r.Create_Async(It.IsAny<Coffeebean>())); //Create a mock of the repository class to be used in the service method

            //Act
            var result = await _coffeeBeanService.CreateCoffeeBean_Async(coffeeBeanDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Coffee Bean", result.Name);
            Assert.AreEqual("This is a test coffee bean", result.Description);
            _mockCoffeeBeanRepository.Verify(r => r.Create_Async(It.IsAny<Coffeebean>()), Times.Once); //Verify that the Create_Async method was called once within the service method
        }
    }
}


