using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IFollowRepository
{
    Task<List<Follow>> GetAllFollowAsync();
    Task<Follow> GetFollowByIdAsync(Guid id);
    Task AddFollowAsync(Follow follow);
    Task UpdateFollowAsync(Follow follow);
    Task DeleteFollowAsync(Guid id);
}