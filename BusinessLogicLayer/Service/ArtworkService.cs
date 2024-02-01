using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

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

    public async Task AddArtworkAsync(Artwork Artwork)
    {
        await _ArtworkRepository.AddArtworkAsync(Artwork);
    }

    public async Task UpdateArtworkAsync(Artwork Artwork)
    {
        await _ArtworkRepository.UpdateArtworkAsync(Artwork);
    }

    public async Task DeleteArtworkAsync(Guid id)
    {
        await _ArtworkRepository.DeleteArtworkAsync(id);
    }
}