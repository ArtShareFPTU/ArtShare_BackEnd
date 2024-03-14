using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;

namespace BusinessLogicLayer.IService;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategoryAsync();
    Task<Category> GetCategoryByIdAsync(Guid id);
    Task<List<Category>> GetCategoryByArtworkId(Guid id);
    Task<List<Artwork>> GetArtworkByCategoryId(Guid id);
    Task<IActionResult> AddCategoryAsync(CategoryCreation Category);
    Task<IActionResult> UpdateCategoryAsync(CategoryUpdate Category);
    Task<IActionResult> DeleteCategoryAsync(Guid id);
}