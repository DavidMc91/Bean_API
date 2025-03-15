using Bean_API.Dtos;
using Bean_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Bean_API.Repository
{
    public class CoffeeBeanRepository : ICoffeeBeanRepository
    {
        private readonly AllthebeansContext _context;

        public CoffeeBeanRepository(AllthebeansContext context)
        {
            _context = context;
        }

        #region Create

        /// <summary>
        /// Create a new coffee bean and save it to the DB
        /// </summary>
        /// <param name="coffeeBean"></param>
        /// <returns></returns>
        public async Task<Coffeebean> Create_Async(Coffeebean coffeeBean)
        {
            _context.Coffeebeans.Add(coffeeBean);
            await _context.SaveChangesAsync();

            return coffeeBean;
        }

        /// <summary>
        /// Add a new coffee bean of the day to the DB (this seperate DB table gives us a history of the beans of the day)
        /// </summary>
        /// <param name="botd"></param>
        /// <returns></returns>
        public async Task AddBotd_Async(Coffeebeanoftheday botd)
        {
            _context.Coffeebeanofthedays.Add(botd);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Read

        /// <summary>
        /// Get the coffee bean of a specific day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<Coffeebean?> GetExistingBotd_Async(DateOnly date)
        {
            return await (from cb in _context.Coffeebeans
                          join botd in _context.Coffeebeanofthedays on cb.Id equals botd.CoffeeBeanId
                          where botd.BotdDate == date
                          select cb).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get all coffee beans, with optional tracking. Enable tracking if you plan to update the entity
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Coffeebean>> Get_All_Async(bool trackChanges)
        {
            var query = _context.Coffeebeans.AsQueryable();

            //No Tracking is more efficient for search only queries
            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = query.Include(cb => cb.Country)
                         .Include(cb => cb.Colour);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Get the coffee bean by ID, return null if not found. Enable tracking if you plan to update the entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Coffeebean?> Get_ByID_Async(string id, bool trackChanges)
        {
            var query = _context.Coffeebeans.AsQueryable();

            //No Tracking is more efficient for search only queries
            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(cb => cb.Id == id)
                         .Include(cb => cb.Country)
                         .Include(cb => cb.Colour);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get coffee beans by search criteria. Tracking is disabled.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Coffeebean?>> Get_BySearch_Async(SearchCoffeeBeanDto search)
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
                .AsNoTracking() //Don't track the entity to improve performance
                .Include(cb => cb.Country) //Include the related country as we need the country name
                .Include(cb => cb.Colour) //Include the related colour as we need the colour name
                .ToListAsync();

        }

        #endregion

        #region Update

        /// <summary>
        /// Save changes to the DB
        /// </summary>
        /// <returns></returns>
        public async Task SaveChanges_Async()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete a coffee bean from the DB
        /// </summary>
        /// <param name="coffeeBean"></param>
        /// <returns></returns>
        public async Task Delete_Async(Coffeebean coffeeBean)
        {
            _context.Coffeebeans.Remove(coffeeBean);
            await _context.SaveChangesAsync();
        }

        #endregion

    }
}
