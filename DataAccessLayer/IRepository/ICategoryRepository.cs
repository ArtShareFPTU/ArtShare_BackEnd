using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoryAsync();
    Task<Category> GetCategoryByIdAsync(Guid id);
    Task<IActionResult> AddCategoryAsync(CategoryCreation category);
    Task<IActionResult> UpdateCategoryAsync(CategoryUpdate category);
    Task<IActionResult> DeleteCategoryAsync(Guid id);
}