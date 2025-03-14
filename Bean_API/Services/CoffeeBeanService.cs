
using Bean_API.Dtos;
using Bean_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace Bean_API.Services
{
    public class CoffeeBeanService : ICoffeeBeanService
    {
        private readonly ILogger<CoffeeBeanService> _logger;
        private readonly AllthebeansContext _context;

        public CoffeeBeanService(ILogger<CoffeeBeanService> logger, AllthebeansContext context)
        {
            _logger = logger;
            _context = context;
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

                //Insert the mapped object
                _context.Coffeebeans.Add(mappedCoffeebean);
                await _context.SaveChangesAsync();

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

        public async Task<ResponseCoffeeBeanDto> GenerateBeanOfTheDay()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            var yesterday = today.AddDays(-1);

            try
            {
                //Ensure a bean of the day doesn't already exist for today
                var existingBotd = await (from cb in _context.Coffeebeans
                                          join botd in _context.Coffeebeanofthedays on cb.Id equals botd.CoffeeBeanId
                                          where botd.BotdDate == today
                                          select new ResponseCoffeeBeanDto
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
                                          })
                                          .FirstOrDefaultAsync();

                if (existingBotd != null)
                {
                    return existingBotd;
                }

                //No bean of the day exists for today, so get yesterday's bean of the day
                var yesterdayBotd = await (from cb in _context.Coffeebeans
                                           where cb.IsBotd == 1
                                           select cb)
                                          .FirstOrDefaultAsync();


                //Get a random coffee bean, excluding yesterday's botd
                var randomCoffeeBean = await _context.Coffeebeans
                   .Where(cb => yesterdayBotd == null || cb.Id != yesterdayBotd.Id)
                   .OrderBy(x => Guid.NewGuid())
                   .FirstAsync();

                if (yesterdayBotd != null) yesterdayBotd.IsBotd = 0;
                randomCoffeeBean.IsBotd = 1;

                //Create our new botd entry, to save to the DB
                var newBotd = new Coffeebeanoftheday
                {
                    CoffeeBeanId = randomCoffeeBean.Id,
                    BotdDate = today
                };

                //Save to the DB and return the coffeebean
                _context.Coffeebeanofthedays.Add(newBotd);

                await _context.SaveChangesAsync();

                return new ResponseCoffeeBeanDto
                {
                    Id = randomCoffeeBean.Id,
                    IndexNum = randomCoffeeBean.IndexNum,
                    IsBotd = Convert.ToBoolean(randomCoffeeBean.IsBotd),
                    Cost = randomCoffeeBean.Cost,
                    Image = randomCoffeeBean.Image,
                    ColourName = randomCoffeeBean.Colour != null ? randomCoffeeBean.Colour.Name : "Unknown",
                    Name = randomCoffeeBean.Name,
                    Description = randomCoffeeBean.Description,
                    CountryName = randomCoffeeBean.Country != null ? randomCoffeeBean.Country.Name : "Unknown"
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
                return await _context.Coffeebeans
                .Select(cb => new ResponseCoffeeBeanDto
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
                })
                .ToListAsync();
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
                return await _context.Coffeebeans
                .Where(cb => cb.Id == id)
                .Select(cb => new ResponseCoffeeBeanDto
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
                })
                .FirstAsync();
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
                var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                var query = _context.Coffeebeans.AsQueryable();

                if (!string.IsNullOrEmpty(search.Name))
                {
                    query = query.Where(x => x.Name.Contains(search.Name));
                }

                if (search.ColourId.HasValue)
                {
                    query = query.Where(x => x.ColourId == search.ColourId);
                }

                if (search.CountryId.HasValue)
                {
                    query = query.Where(x => x.CountryId == search.CountryId);
                }

                if (search.IsBotd.HasValue)
                {
                    query = query.Join(_context.Coffeebeanofthedays, cb => cb.Id, botd => botd.CoffeeBeanId, (cb, botd) => new { cb, botd })
                                 .Where(x => x.botd.BotdDate == today)
                                 .Select(x => x.cb);
                }

                if (search.MaxCost.HasValue)
                {
                    query = query.Where(x => x.Cost <= search.MaxCost);
                }

                return await query
                    .Select(cb => new ResponseCoffeeBeanDto
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
                    })
                    .ToListAsync();
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
                var existingBean = await _context.Coffeebeans.FindAsync(id);
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
                await _context.SaveChangesAsync();

                return coffeebean;
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
                var coffeeBean = await _context.Coffeebeans.FindAsync(id);
                if (coffeeBean == null) return false;

                _context.Coffeebeans.Remove(coffeeBean);
                await _context.SaveChangesAsync();
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
