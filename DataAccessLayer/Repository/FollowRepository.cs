using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.Repository;

public class FollowRepository : IFollowRepository
{
    private readonly ArtShareContext _context;

    public FollowRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Follow>> GetAllFollowAsync()
    {
        return await _context.Follows.ToListAsync();
    }

    public async Task<Follow> GetFollowByIdAsync(Guid id)
    {
        return await _context.Follows.FindAsync(id);
    }

    public async Task AddFollowAsync(Follow follow)
    {
        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFollowAsync(Follow follow)
    {
        _context.Entry(follow).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFollowAsync(Guid id)
    {
        var follow = await _context.Follows.FindAsync(id);
        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();
    }
}