using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Like;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ILikeRepository
{
    Task<List<Like>> GetAllLikeAsync();
    Task<Like> GetLikeByIdAsync(Guid id);
    Task<List<Like>> GetLikeByArtworkId(Guid id);
    Task<IActionResult> AddLikeAsync(LikeCreation likeCreation);
    Task UpdateLikeAsync(Like like);
    Task DeleteLikeAsync(Guid id,Guid artWorkId);
    Task<bool> CheckIfLikeExists(Guid accountId, Guid artworkId);
    Task<Guid?> GetLikeId(Guid accountId, Guid artworkId);
}