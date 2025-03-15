using Bean_API.Dtos;
using Bean_API.Models;

namespace Bean_API.Services
{
    public interface ICoffeeBeanService
    {
        public Task<CreateCoffeeBeanDto> CreateCoffeeBean_Async(CreateCoffeeBeanDto coffeeBean);

        public Task<ResponseCoffeeBeanDto?> GenerateBeanOfTheDay_Async();

        public Task<ResponseCoffeeBeanDto?> GetCoffeeBean_ByID_Async(string id);

        public Task<IEnumerable<ResponseCoffeeBeanDto?>> GetCoffeeBean_BySearch_Async(SearchCoffeeBeanDto search);

        public Task<IEnumerable<ResponseCoffeeBeanDto>> GetCoffeeBean_All_Async();

        public Task<CreateCoffeeBeanDto?> UpdateCoffeeBean_ByID_Async(string id, CreateCoffeeBeanDto coffeebean);

        public Task<bool> DeleteCoffeeBean_ByID_Async(string id);
        
    }
}
