using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ILikeRepository
{
    Task<List<Like>> GetAllLikeAsync();
    Task<Like> GetLikeByIdAsync(Guid id);
    Task AddLikeAsync(Like like);
    Task UpdateLikeAsync(Like like);
    Task DeleteLikeAsync(Guid id);
}