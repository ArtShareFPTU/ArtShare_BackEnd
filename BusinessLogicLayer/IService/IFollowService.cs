using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.IService;

public interface IFollowService
{
    Task<List<Follow>> GetAllFollowAsync();
    Task<Follow> GetFollowByIdAsync(Guid id);
    Task AddFollowAsync(Follow Follow);
    Task UpdateFollowAsync(Follow Follow);
    Task DeleteFollowAsync(Guid id);
}