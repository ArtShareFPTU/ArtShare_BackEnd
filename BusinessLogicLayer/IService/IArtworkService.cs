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
    Task<List<ArtworkCategory>> GetArtworkCategoryByArtworkId(Guid id);
    Task<List<ArtworkCategory>> GetArtworkCategoryByCategoryId(Guid id);
    Task<List<ArtworkCategory>> GetArtworkCategories();

    Task<List<ArtworkTag>> GetArtworkTagByArtworkId(Guid id);
    Task<List<ArtworkTag>> GetArtworkTagByTagId(Guid id);
    Task<List<ArtworkTag>> GetArtworkTags();
    Task<List<Artwork>> GetArtworkFromSearch(string search);
}