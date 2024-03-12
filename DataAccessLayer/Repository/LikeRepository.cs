using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Like;

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

    public async Task<IActionResult> AddLikeAsync(LikeCreation likeCreation)
    {
        var likeToAdd = new Like
        {
            Id = Guid.NewGuid(),
            AccountId = likeCreation.AccountId,
            ArtworkId = likeCreation.ArtworkId,
            Artwork = _context.Artworks.FirstOrDefault(a => a.Id == likeCreation.ArtworkId),
            CreateDate = DateTime.Now
        };
        likeToAdd.Artwork.Likes += 1;
        _context.Likes.Add(likeToAdd);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
    }

    public async Task UpdateLikeAsync(Like like)
    {
        _context.Entry(like).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLikeAsync(Guid id, Guid ArtworkId)
    {
        var like = await _context.Likes.FindAsync(id);
        if (like != null)
        {
            var artwork = await _context.Artworks.FindAsync(ArtworkId);

            if (artwork != null)
            {
                artwork.Likes -= 1;
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task<Guid?> GetLikeId(Guid accountId, Guid artworkId)
    {
        // Tìm like có accountId và artworkId t??ng ?ng trong c? s? d? li?u
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.AccountId == accountId && l.ArtworkId == artworkId);

        // N?u tìm th?y like, tr? v? Id c?a like, ng??c l?i tr? v? null
        if (like != null)
        {
            return like.Id;
        }
        else
        {
            return null;
        }
    }


    public async Task<bool> CheckIfLikeExists(Guid accountId, Guid artworkId)
    {
        // Ki?m tra xem có b?n ghi nào trong c? s? d? li?u có tài kho?n và ID tác ph?m t??ng ?ng không
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.AccountId == accountId && l.ArtworkId == artworkId);

        // N?u có, tr? v? true, ng??c l?i tr? v? false
        return like != null;
    }
}