using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.Repository;

public class LikeRepository : ILikeRepository
{
    private readonly ArtShareContext _context;

    public LikeRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Like>> GetAllLikeAsync()
    {
        return await _context.Likes.ToListAsync();
    }

    public async Task<Like> GetLikeByIdAsync(Guid id)
    {
        return await _context.Likes.FindAsync(id);
    }

    public async Task<List<Like>> GetLikeByArtworkId(Guid id)
    {
        return await _context.Likes.Where(c => c.ArtworkId.Equals(id)).ToListAsync();
    }

    public async Task AddLikeAsync(Like like)
    {
        _context.Likes.Add(like);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLikeAsync(Like like)
    {
        _context.Entry(like).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLikeAsync(Guid id)
    {
        var like = await _context.Likes.FindAsync(id);
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }
}