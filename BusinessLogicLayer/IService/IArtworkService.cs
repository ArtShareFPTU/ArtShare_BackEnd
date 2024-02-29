using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using ModelLayer.DTOS.Response;

namespace BusinessLogicLayer.IService;

public interface IArtworkService
{
    Task<List<Artwork>> GetAllArtworkAsync();
    Task<Artwork> GetArtworkByIdAsync(Guid id);
    Task<IActionResult> AddArtworkAsync(ArtworkCreation Artwork);
    Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate Artwork);
    Task<IActionResult> DeleteArtworkAsync(Guid id);
    Task<List<ArtworkRespone>> GetArtworkByArtistId(Guid artistId);
}