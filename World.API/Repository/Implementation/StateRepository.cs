using Microsoft.EntityFrameworkCore;
using World.API.Data;
using World.API.Models;
using World.API.Repository.IRepository;

namespace World.API.Repository.Implementation
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StateRepository(ApplicationDbContext dbContext) : base(dbContext) 
        {
               _dbContext = dbContext;
        }
 

        public async Task Update(State state)
        {
            _dbContext.States.Update(state);
            await _dbContext.SaveChangesAsync();
        }
    }
}
