using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        return await _context.Artworks.Include(a => a.ArtworkCategories).Include(a => a.ArtworkTags).Include(a => a.Comments).Include(a => a.LikesNavigation).Include(a => a.OrderDetails).ToListAsync();
    }

    public async Task<Artwork> GetArtworkByIdAsync(Guid id)
    {
        return await _context.Artworks.Include(a => a.ArtworkCategories).Include(a => a.ArtworkTags).Include(a => a.Comments).Include(a => a.LikesNavigation).Include(a => a.OrderDetails).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public async Task<IActionResult> AddArtworkAsync(ArtworkCreation artwork)
    {
        var imageExist = await _context.Artworks.FirstOrDefaultAsync(c => c.AccountId.Equals(artwork.AccountId) && c.Title.ToLower().Equals(artwork.Title.ToLower()));
        if (imageExist != null)
        {
            return new StatusCodeResult(409);
        }
        Artwork createArtwork = new Artwork {
            Id = Guid.NewGuid(),
            Title = artwork.Title,
            AccountId = artwork.AccountId,
            Description = artwork.Description,
            Url = artwork.Url,
            Likes = artwork.Likes,
            Fee = artwork.Fee,
            Status = artwork.Status
        };
        _context.Artworks.Add(createArtwork);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
    }

    public async Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate artwork)
    {
        var imageExist = await _context.Artworks.FirstOrDefaultAsync(c => c.AccountId.Equals(artwork.AccountId) && c.Title.ToLower().Equals(artwork.Title.ToLower()));
        if (imageExist != null)
        {
            return new StatusCodeResult(409);
        }
        imageExist = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(artwork.Id));
        if(!string.IsNullOrEmpty(artwork.Title)) imageExist.Title = artwork.Title;
        if (!string.IsNullOrEmpty(artwork.Url)) imageExist.Url = artwork.Url;
        if (!string.IsNullOrEmpty(artwork.Description)) imageExist.Description = artwork.Description;
        if(artwork.Fee != null) imageExist.Fee = artwork.Fee;
        if(artwork.Status != null) imageExist.Status = artwork.Status;
        if (artwork.Likes != null) artwork.Likes = artwork.Likes;
        if(artwork.AccountId != null) imageExist.AccountId = artwork.AccountId;

        _context.Artworks.Update(imageExist);

        _context.Entry(imageExist).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return new StatusCodeResult(200);
    }

    public async Task<IActionResult> DeleteArtworkAsync(Guid id)
    {
        var artwork = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(id));
        if (artwork == null) return new StatusCodeResult(404);
        _context.Artworks.Remove(artwork);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(204);
    }
}