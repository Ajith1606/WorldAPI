using World.API.Models;

namespace World.API.Repository.IRepository
{
    public interface IStateRepository : IGenericRepository<State>
    {
        Task Update(State state);
       
    }
}
