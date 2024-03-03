using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.IService;

public interface ILikeService
{
    Task<List<Like>> GetAllLikeAsync();
    Task<Like> GetLikeByIdAsync(Guid id);
    Task<List<Like>> GetLikeByArtworkId(Guid id);
    Task AddLikeAsync(Like Like);
    Task UpdateLikeAsync(Like Like);
    Task DeleteLikeAsync(Guid id);
}