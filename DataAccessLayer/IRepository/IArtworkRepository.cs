using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IArtworkRepository
{
    Task<List<Artwork>> GetAllArtworkAsync();
    Task<Artwork> GetArtworkByIdAsync(Guid id);
    Task<IActionResult> AddArtworkAsync(ArtworkCreation artwork);
    Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate artwork);
    Task<IActionResult> DeleteArtworkAsync(Guid id);
}