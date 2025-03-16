

using Bean_API.Dtos;
using Bean_API.Models;
using Bean_API.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Bean_APITest.RepositoryTests
{
    [TestClass]
    public class CoffeeBeanRepositoryTests
    {
        public required Mock<DbSet<Coffeebean>> _mockSet;
        public required Mock<AllthebeansContext> _mockContext;
        public required CoffeeBeanRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            //Setup the objects and mock dependencies for the test methods
            _mockSet = new Mock<DbSet<Coffeebean>>();
            _mockContext = new Mock<AllthebeansContext>();
            _mockContext.Setup(c => c.Coffeebeans).Returns(_mockSet.Object);
            _repository = new CoffeeBeanRepository(_mockContext.Object);
        }

        [TestMethod]
        public async Task Create_Async_ShouldReturnCoffeeBean_WhenAddedSuccessfully()
        {
            //Arrange
            var coffeeBean = new Coffeebean()
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
           
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); //Mock that 1 row was affected, indicating success

            //Act
            var result = await _repository.Create_Async(coffeeBean);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("66a374596122a40616cb8599", result.Id);
            Assert.AreEqual("Test Coffee Bean", result.Name);
            Assert.AreEqual("This is a test coffee bean", result.Description);           
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once); //Verify that SaveChangesAsync was called once
        }
    }
}

 
