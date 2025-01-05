using Microsoft.EntityFrameworkCore;
using World.API.Data;
using World.API.DTOs.Country;
using World.API.Models;
using World.API.Repository.IRepository;

namespace World.API.Repository.Implementation
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CountryRepository(ApplicationDbContext dbContext) :base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task Update(Country country)
        {
            _dbContext.Countries.Update(country);
            await _dbContext.SaveChangesAsync();
        }

      
    }
}
