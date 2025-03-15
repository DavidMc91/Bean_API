
using Bean_API.Dtos;
using Bean_API.Models;
using Microsoft.EntityFrameworkCore;
using Bean_API.Repository;

namespace Bean_API.Services
{
    public class CoffeeBeanService : ICoffeeBeanService
    {
        private readonly ILogger<CoffeeBeanService> _logger;
        private readonly ICoffeeBeanRepository _repository;

        public CoffeeBeanService(ILogger<CoffeeBeanService> logger, ICoffeeBeanRepository coffeeBeanRepository)
        {
            _logger = logger;
            _repository = coffeeBeanRepository;
        }

        #region Create

        public async Task<CreateCoffeeBeanDto> CreateCoffeeBean_Async(CreateCoffeeBeanDto coffeebean)
        {
            try
            {
                //Map from DTO to Model (Can use AutoMapper for this but just done it manually for this demo)
                var mappedCoffeebean = new Coffeebean
                {
                    Id = coffeebean.Id,
                    IndexNum = coffeebean.IndexNum,
                    IsBotd = coffeebean.IsBotd,
                    Cost = coffeebean.Cost,
                    Image = coffeebean.Image,
                    ColourId = coffeebean.ColourId,
                    Name = coffeebean.Name,
                    Description = coffeebean.Description,
                    CountryId = coffeebean.CountryId
                };

                await _repository.Create_Async(mappedCoffeebean);

                return coffeebean;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Create failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }
        }

        public async Task<ResponseCoffeeBeanDto?> GenerateBeanOfTheDay_Async()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            var yesterday = today.AddDays(-1);

            try
            {
                //Ensure a bean of the day doesn't already exist for today. If it does, return it
                var existingBotd = await _repository.GetExistingBotd_Async(today);
                if (existingBotd != null)
                {
                    return new ResponseCoffeeBeanDto
                    {
                        Id = existingBotd.Id,
                        IndexNum = existingBotd.IndexNum,
                        IsBotd = Convert.ToBoolean(existingBotd.IsBotd),
                        Cost = existingBotd.Cost,
                        Image = existingBotd.Image,
                        ColourName = existingBotd.Colour != null ? existingBotd.Colour.Name : "Unknown",
                        Name = existingBotd.Name,
                        Description = existingBotd.Description,
                        CountryName = existingBotd.Country != null ? existingBotd.Country.Name : "Unknown"
                    };
                }

                //No bean of the day exists for today, so lets try to create a new one. First we need to get all the coffee beans
                var allCoffeeBeans = await _repository.Get_All_Async(true);
                if (allCoffeeBeans == null || allCoffeeBeans.Count() == 0) return null;

                //Get yesterday's bean of the day
                var yesterdayBotd = allCoffeeBeans
                    .Where(cb => cb.IsBotd == 1)
                    .FirstOrDefault();

                //Get a random coffee bean, excluding yesterday's botd
                var todaysBotd = allCoffeeBeans
                    .Where(cb => yesterdayBotd == null || cb.Id != yesterdayBotd?.Id)
                    .OrderBy(x => Guid.NewGuid())
                    .First();

                //Update the botd status for yesterday's bean and today's bean
                if (yesterdayBotd != null) yesterdayBotd.IsBotd = 0;
                todaysBotd.IsBotd = 1;

                //Create our new botd entry, to save to the DB
                var newBotd = new Coffeebeanoftheday
                {
                    CoffeeBeanId = todaysBotd.Id,
                    BotdDate = today
                };

                //Save to the DB and return the coffee bean of the day in a DTO
                await _repository.AddBotd_Async(newBotd);
                await _repository.SaveChanges_Async();

                return new ResponseCoffeeBeanDto
                {
                    Id = todaysBotd.Id,
                    IndexNum = todaysBotd.IndexNum,
                    IsBotd = Convert.ToBoolean(todaysBotd.IsBotd),
                    Cost = todaysBotd.Cost,
                    Image = todaysBotd.Image,
                    ColourName = todaysBotd.Colour != null ? todaysBotd.Colour.Name : "Unknown",
                    Name = todaysBotd.Name,
                    Description = todaysBotd.Description,
                    CountryName = todaysBotd.Country != null ? todaysBotd.Country.Name : "Unknown"
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Create failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }
        }

        #endregion

        #region Read
        public async Task<IEnumerable<ResponseCoffeeBeanDto>> GetCoffeeBean_All_Async()
        {
            try
            {
                //Get all the coffee beans from the DB
                var coffeeBeanList = await _repository.Get_All_Async(false);
             
                //Map to a DTO model and return to caller
                return coffeeBeanList.Select(cb => new ResponseCoffeeBeanDto
                {
                    Id = cb.Id,
                    IndexNum = cb.IndexNum,
                    IsBotd = Convert.ToBoolean(cb.IsBotd),
                    Cost = cb.Cost,
                    Image = cb.Image,
                    ColourName = cb.Colour != null ? cb.Colour.Name : "Unknown",
                    Name = cb.Name,
                    Description = cb.Description,
                    CountryName = cb.Country != null ? cb.Country.Name : "Unknown"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }

        }

        public async Task<ResponseCoffeeBeanDto?> GetCoffeeBean_ByID_Async(string id)
        {
            try
            {
                //Get the coffee bean by ID. It will return null if not found
                var coffeeBean = await _repository.Get_ByID_Async(id, false);

                return coffeeBean != null ? new ResponseCoffeeBeanDto
                {
                    Id = coffeeBean.Id,
                    IndexNum = coffeeBean.IndexNum,
                    IsBotd = Convert.ToBoolean(coffeeBean.IsBotd),
                    Cost = coffeeBean.Cost,
                    Image = coffeeBean.Image,
                    ColourName = coffeeBean.Colour != null ? coffeeBean.Colour.Name : "Unknown",
                    Name = coffeeBean.Name,
                    Description = coffeeBean.Description,
                    CountryName = coffeeBean.Country != null ? coffeeBean.Country.Name : "Unknown"
                } : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }      
        }

        public async Task<IEnumerable<ResponseCoffeeBeanDto?>> GetCoffeeBean_BySearch_Async(SearchCoffeeBeanDto search)
        {
            try
            {
                var coffeBeanList = await _repository.Get_BySearch_Async(search);

                return coffeBeanList.Select(cb => cb!= null ? new ResponseCoffeeBeanDto
                {
                    Id = cb.Id,
                    IndexNum = cb.IndexNum,
                    IsBotd = Convert.ToBoolean(cb.IsBotd),
                    Cost = cb.Cost,
                    Image = cb.Image,
                    ColourName = cb.Colour != null ? cb.Colour.Name : "Unknown",
                    Name = cb.Name,
                    Description = cb.Description,
                    CountryName = cb.Country != null ? cb.Country.Name : "Unknown"
                } : null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }
        }

        #endregion

        #region Update

        public async Task<CreateCoffeeBeanDto?> UpdateCoffeeBean_ByID_Async(string id, CreateCoffeeBeanDto coffeebean)
        {
            try
            {
                //Find existing bean
                var existingBean = await _repository.Get_ByID_Async(id, true);
                if (existingBean == null) return null;

                //Update properties
                existingBean.Id = coffeebean.Id;
                existingBean.IndexNum = coffeebean.IndexNum;
                existingBean.IsBotd = coffeebean.IsBotd;
                existingBean.Cost = coffeebean.Cost;
                existingBean.Image = coffeebean.Image;
                existingBean.ColourId = coffeebean.ColourId;
                existingBean.Name = coffeebean.Name;
                existingBean.Description = coffeebean.Description;
                existingBean.CountryId = coffeebean.CountryId;

                //_context.Coffeebeans.Update(existingBean);
                await _repository.SaveChanges_Async();

                return coffeebean;

                ////Find existing bean
                //var existingBean = await _context.Coffeebeans.FindAsync(id);
                //if (existingBean == null) return null;

                ////Update properties
                //existingBean.Id = coffeebean.Id;
                //existingBean.IndexNum = coffeebean.IndexNum;
                //existingBean.IsBotd = coffeebean.IsBotd;
                //existingBean.Cost = coffeebean.Cost;
                //existingBean.Image = coffeebean.Image;
                //existingBean.ColourId = coffeebean.ColourId;
                //existingBean.Name = coffeebean.Name;
                //existingBean.Description = coffeebean.Description;
                //existingBean.CountryId = coffeebean.CountryId;

                ////_context.Coffeebeans.Update(existingBean);
                //await _context.SaveChangesAsync();

                //return coffeebean;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Update failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }           
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteCoffeeBean_ByID_Async(string id)
        {
            try
            {
                var coffeeBean = await _repository.Get_ByID_Async(id, true);
                if (coffeeBean == null) return false;

                await _repository.Delete_Async(coffeeBean);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Delete failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                throw;
            }
        }

        #endregion
    }
}
