using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;

namespace BusinessLogicLayer.Service;

public class ArtworkService : IArtworkService
{
    private readonly IArtworkRepository _ArtworkRepository;

    public ArtworkService(IArtworkRepository ArtworkRepository)
    {
        _ArtworkRepository = ArtworkRepository;
    }

    public async Task<List<Artwork>> GetAllArtworkAsync()
    {
        return await _ArtworkRepository.GetAllArtworkAsync();
    }

    public async Task<Artwork> GetArtworkByIdAsync(Guid id)
    {
        return await _ArtworkRepository.GetArtworkByIdAsync(id);
    }

    public async Task<IActionResult> AddArtworkAsync(ArtworkCreation Artwork)
    {
        return await _ArtworkRepository.AddArtworkAsync(Artwork);
    }

    public async Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate Artwork)
    {
        return await _ArtworkRepository.UpdateArtworkAsync(Artwork);
    }

    public async Task<IActionResult> DeleteArtworkAsync(Guid id)
    {
        return await _ArtworkRepository.DeleteArtworkAsync(id);
    }
}