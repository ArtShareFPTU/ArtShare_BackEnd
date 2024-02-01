using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

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

    public async Task<Category> GetCategoryByIdAsync(Guid id)
    {
        return await _CategoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task AddCategoryAsync(Category Category)
    {
        await _CategoryRepository.AddCategoryAsync(Category);
    }

    public async Task UpdateCategoryAsync(Category Category)
    {
        await _CategoryRepository.UpdateCategoryAsync(Category);
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        await _CategoryRepository.DeleteCategoryAsync(id);
    }
}