using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Like;

namespace BusinessLogicLayer.IService;

public interface ILikeService
{
    Task<List<Like>> GetAllLikeAsync();
    Task<Like> GetLikeByIdAsync(Guid id);
    Task<List<Like>> GetLikeByArtworkId(Guid id);
    Task<IActionResult> AddLikeAsync(LikeCreation likeCreation);
    Task UpdateLikeAsync(Like Like);
    Task DeleteLikeAsync(Guid id, Guid artWorkId);
    Task<bool> CheckIfLikeExists(Guid accountId, Guid artworkId);
    Task<Guid?> GetLikeId(Guid accountId, Guid artworkId);
}