using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Like;

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

    public async Task<List<Like>> GetLikeByArtworkId(Guid id)
    {
        return await _LikeRepository.GetLikeByArtworkId(id);
    }

    public async Task<Like> GetLikeByIdAsync(Guid id)
    {
        return await _LikeRepository.GetLikeByIdAsync(id);
    }

    public async Task<IActionResult> AddLikeAsync(LikeCreation likeCreation)
    {
        return await _LikeRepository.AddLikeAsync(likeCreation);
    }

    public async Task UpdateLikeAsync(Like Like)
    {
        await _LikeRepository.UpdateLikeAsync(Like);
    }

    public async Task DeleteLikeAsync(Guid id,Guid artWorkId)
    {
        await _LikeRepository.DeleteLikeAsync(id, artWorkId);
    }

    public async Task<bool> CheckIfLikeExists(Guid accountId, Guid artworkId)
    {
        return await _LikeRepository.CheckIfLikeExists(accountId,artworkId);
    }

    public async Task<Guid?> GetLikeId(Guid accountId, Guid artworkId)
    {
        return await _LikeRepository.GetLikeId(accountId,artworkId);
    }
}