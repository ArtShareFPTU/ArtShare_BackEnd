using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.IService;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategoryAsync();
    Task<Category> GetCategoryByIdAsync(Guid id);
    Task AddCategoryAsync(Category Category);
    Task UpdateCategoryAsync(Category Category);
    Task DeleteCategoryAsync(Guid id);
}