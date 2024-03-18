using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Pagination;
using ModelLayer.DTOS.Request.Artwork;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IArtworkRepository
{
    Task<List<Artwork>> GetAllArtworkAsync();
    Task<Artwork> GetArtworkByIdAsync(Guid id);
    Task<IActionResult> AddArtworkAsync(ArtworkCreation artwork);
    Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate artwork);
    Task<IActionResult> DeleteArtworkAsync(Guid id);
    Task<List<Artwork>> GetArtworkByArtistId(Guid artistId);
    Task<IActionResult> AddCategory4Artwork(ArtworkCategoryAddition artworkCategoryAddition);
    Task<IActionResult> UpdateCategory4Artwork(ArtworkCategoryUpdate artworkCategoryUpdate);
    Task<IActionResult> AddTag4Artwork(ArtworkTagAddition artworkTagAddition);
    Task<IActionResult> UpdateTag4Artwork(ArtworkTagUpdate artworkTagUpdate);
    Task<IActionResult> RemoveTag4Artwork(Guid id);
    Task<IActionResult> RemoveCategory4Artwork(Guid id);
    Task<List<ArtworkCategory>> GetArtworkCategoryByArtworkId(Guid id);
     Task<List<ArtworkCategory>> GetArtworkCategoryByCategoryId(Guid id);
     Task<List<ArtworkCategory>> GetArtworkCategories();

     Task<List<ArtworkTag>> GetArtworkTagByArtworkId(Guid id);
     Task<List<ArtworkTag>> GetArtworkTagByTagId(Guid id);
     Task<List<ArtworkTag>> GetArtworkTags();

    Task<List<Artwork>> GetArtworkFromSearch(string search);
    Task<Pagination<Artwork>> ToPagination(int pageindex = 0);

}