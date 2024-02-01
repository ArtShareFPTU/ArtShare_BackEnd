using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.IService;

public interface IArtworkService
{
    Task<List<Artwork>> GetAllArtworkAsync();
    Task<Artwork> GetArtworkByIdAsync(Guid id);
    Task AddArtworkAsync(Artwork Artwork);
    Task UpdateArtworkAsync(Artwork Artwork);
    Task DeleteArtworkAsync(Guid id);
}