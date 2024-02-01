using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.Repository;

public class ArtworkRepository : IArtworkRepository
{
    private readonly ArtShareContext _context;

    public ArtworkRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Artwork>> GetAllArtworkAsync()
    {
        return await _context.Artworks.ToListAsync();
    }

    public async Task<Artwork> GetArtworkByIdAsync(Guid id)
    {
        return await _context.Artworks.FindAsync(id);
    }

    public async Task AddArtworkAsync(Artwork artwork)
    {
        _context.Artworks.Add(artwork);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateArtworkAsync(Artwork artwork)
    {
        _context.Entry(artwork).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArtworkAsync(Guid id)
    {
        var artwork = await _context.Artworks.FindAsync(id);
        _context.Artworks.Remove(artwork);
        await _context.SaveChangesAsync();
    }
}