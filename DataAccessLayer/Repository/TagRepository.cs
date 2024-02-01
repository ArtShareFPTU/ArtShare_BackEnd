using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.Repository;

public class TagRepository : ITagRepository
{
    private readonly ArtShareContext _context;

    public TagRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Tag>> GetAllTagAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task<Tag> GetTagByIdAsync(Guid id)
    {
        return await _context.Tags.FindAsync(id);
    }

    public async Task AddTagAsync(Tag tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTagAsync(Tag tag)
    {
        _context.Entry(tag).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTagAsync(Guid id)
    {
        var tag = await _context.Tags.FindAsync(id);
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }
}