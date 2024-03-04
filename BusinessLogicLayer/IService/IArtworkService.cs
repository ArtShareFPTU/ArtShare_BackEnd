using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using ModelLayer.DTOS.Response;

namespace BusinessLogicLayer.IService;

public interface IArtworkService
{
    Task<List<Artwork>> GetAllArtworkAsync();
    Task<ArtworkRespone> GetArtworkByIdAsync(Guid id);
    Task<IActionResult> AddArtworkAsync(ArtworkCreation Artwork);
    Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate Artwork);
    Task<IActionResult> DeleteArtworkAsync(Guid id);
    Task<IActionResult> UpdateCategory4Artwork(ArtworkCategoryUpdate artworkCategoryUpdate);
    Task<IActionResult> AddCategory4Artwork(ArtworkCategoryAddition artworkCategoryAddition);
    Task<IActionResult> UpdateTag4Artwork(ArtworkTagUpdate artworkTagUpdate);
    Task<IActionResult> AddTag4Artwork(ArtworkTagAddition artworkTagAddition);
    Task<IActionResult> RemoveTag4Artwork(Guid id);
    Task<IActionResult> RemoveCategory4Artwork(Guid id);
    Task<List<ArtworkRespone>> GetArtworkByArtistId(Guid artistId);
    Task<List<Artwork>> GetArtworkFromSearch(string search);
}