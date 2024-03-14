using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;

namespace BusinessLogicLayer.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _CategoryRepository;

    public CategoryService(ICategoryRepository CategoryRepository)
    {
        _CategoryRepository = CategoryRepository;
    }

    public async Task<List<Category>> GetAllCategoryAsync()
    {
        return await _CategoryRepository.GetAllCategoryAsync();
    }

    public async Task<List<Category>> GetCategoryByArtworkId(Guid id)
    {
        return await _CategoryRepository.GetCategoryByArtworkId(id);
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id)
    {
        return await _CategoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task<IActionResult> AddCategoryAsync(CategoryCreation Category)
    {
        return await _CategoryRepository.AddCategoryAsync(Category);
    }

    public async Task<IActionResult> UpdateCategoryAsync(CategoryUpdate Category)
    {
        return await _CategoryRepository.UpdateCategoryAsync(Category);
    }

    public async Task<IActionResult> DeleteCategoryAsync(Guid id)
    {
        return await _CategoryRepository.DeleteCategoryAsync(id);
    }

    public async Task<List<Artwork>> GetArtworkByCategoryId(Guid id)
    {
        return await _CategoryRepository.GetArtworkByCategoryId(id);
    }
}