using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.Service;

public class FollowService : IFollowService
{
    private readonly IFollowRepository _FollowRepository;

    public FollowService(IFollowRepository FollowRepository)
    {
        _FollowRepository = FollowRepository;
    }

    public async Task<List<Follow>> GetAllFollowAsync()
    {
        return await _FollowRepository.GetAllFollowAsync();
    }

    public async Task<Follow> GetFollowByIdAsync(Guid id)
    {
        return await _FollowRepository.GetFollowByIdAsync(id);
    }

    public async Task AddFollowAsync(Follow Follow)
    {
        await _FollowRepository.AddFollowAsync(Follow);
    }

    public async Task UpdateFollowAsync(Follow Follow)
    {
        await _FollowRepository.UpdateFollowAsync(Follow);
    }

    public async Task DeleteFollowAsync(Guid id)
    {
        await _FollowRepository.DeleteFollowAsync(id);
    }
}