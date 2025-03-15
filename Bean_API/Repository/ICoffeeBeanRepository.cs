using Bean_API.Dtos;
using Bean_API.Models;

namespace Bean_API.Repository
{
    public interface ICoffeeBeanRepository
    {
        public Task<Coffeebean> Create_Async(Coffeebean coffeeBean);
        public Task<Coffeebean?> GetExistingBotd_Async(DateOnly date);
        public Task AddBotd_Async(Coffeebeanoftheday botd);
        public Task SaveChanges_Async();
        public Task<IEnumerable<Coffeebean>> Get_All_Async(bool trackChanges);
        public Task<Coffeebean?> Get_ByID_Async(string id, bool trackChanges);
        public Task<IEnumerable<Coffeebean?>> Get_BySearch_Async(SearchCoffeeBeanDto search);
        public Task Delete_Async(Coffeebean coffeeBean);
    }
}
