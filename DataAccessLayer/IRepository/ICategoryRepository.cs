using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoryAsync();
    Task<Category> GetCategoryByIdAsync(Guid id);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Guid id);
}