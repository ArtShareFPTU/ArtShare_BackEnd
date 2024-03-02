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
    Task<IActionResult> AddCategory4Artwork(ArtworkCategoryAddition artworkCategoryAddition);
    Task<IActionResult> UpdateCategory4Artwork(ArtworkCategoryUpdate artworkCategoryUpdate);
    Task<IActionResult> AddTag4Artwork(ArtworkTagAddition artworkTagAddition);
    Task<IActionResult> UpdateTag4Artwork(ArtworkTagUpdate artworkTagUpdate);
    Task<IActionResult> RemoveTag4Artwork(Guid id);
    Task<IActionResult> RemoveCategory4Artwork(Guid id);
}