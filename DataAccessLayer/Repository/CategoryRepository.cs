using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;

namespace DataAccessLayer.BussinessObject.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ArtShareContext _context;

    public CategoryRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Category>> GetAllCategoryAsync()
    {
        return await _context.Categories.Include(c => c.ArtworkCategories).ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id)
    {
        return await _context.Categories.Include(c => c.ArtworkCategories).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public async Task<List<Category>> GetCategoryByArtworkId(Guid id)
    {
        var artworkCategories = _context.ArtworkCategories.Where(c => c.ArtworkId.Equals(id)).ToList();

        var categoryIDs = new HashSet<Guid>();
        foreach (var item in artworkCategories) categoryIDs.Add((Guid)item.CategoryId);
        var categories = new List<Category>();
        foreach (var category in categoryIDs)
            categories.Add(await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(category)));
        return categories;
    }

    public async Task<IActionResult> AddCategoryAsync(CategoryCreation category)
    {
        var categoryExist = await _context.Categories.AnyAsync(c => c.Title.ToLower().Equals(category.Title.ToLower()));
        if (categoryExist) return new StatusCodeResult(409);
        var categoryToAdd = new Category
        {
            Id = Guid.NewGuid(),
            Title = category.Title,
            CreateDate = DateTime.Now
        };
        _context.Categories.Add(categoryToAdd);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
    }

    public async Task<IActionResult> UpdateCategoryAsync(CategoryUpdate category)
    {
        var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        if (categoryExist == null) return new StatusCodeResult(409);
        categoryExist.Title = category.Title;
        _context.Categories.Update(categoryExist);
        _context.Entry(categoryExist).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return new StatusCodeResult(200);
    }

    public async Task<IActionResult> DeleteCategoryAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return new StatusCodeResult(404);
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(204);
    }
}