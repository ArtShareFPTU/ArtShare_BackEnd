using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IArtworkRepository
{
    Task<List<Artwork>> GetAllArtworkAsync();
    Task<Artwork> GetArtworkByIdAsync(Guid id);
    Task AddArtworkAsync(Artwork artwork);
    Task UpdateArtworkAsync(Artwork artwork);
    Task DeleteArtworkAsync(Guid id);
}