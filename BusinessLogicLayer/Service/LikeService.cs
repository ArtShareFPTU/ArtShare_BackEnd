using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.Service;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _LikeRepository;

    public LikeService(ILikeRepository LikeRepository)
    {
        _LikeRepository = LikeRepository;
    }

    public async Task<List<Like>> GetAllLikeAsync()
    {
        return await _LikeRepository.GetAllLikeAsync();
    }

    public async Task<Like> GetLikeByIdAsync(Guid id)
    {
        return await _LikeRepository.GetLikeByIdAsync(id);
    }

    public async Task AddLikeAsync(Like Like)
    {
        await _LikeRepository.AddLikeAsync(Like);
    }

    public async Task UpdateLikeAsync(Like Like)
    {
        await _LikeRepository.UpdateLikeAsync(Like);
    }

    public async Task DeleteLikeAsync(Guid id)
    {
        await _LikeRepository.DeleteLikeAsync(id);
    }
}