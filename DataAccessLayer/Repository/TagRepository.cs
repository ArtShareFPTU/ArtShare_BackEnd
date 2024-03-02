using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Tags;

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
        return await _context.Tags.Include(c => c.ArtworkTags).ToListAsync();
    }

    public async Task<Tag> GetTagByIdAsync(Guid id)
    {
        return await _context.Tags.Include(c => c.ArtworkTags).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public async Task<List<Tag>> GetTagByArtworkIdAsync(Guid id)
    {
        var artworkTags = _context.ArtworkTags.Where(c => c.ArtworkId.Equals(id)).ToList();
        var tagIDs = new HashSet<Guid>();
        foreach (var item in artworkTags) tagIDs.Add((Guid)item.TagId);
        var tags = new List<Tag>();
        foreach (var tagId in tagIDs) tags.Add(await _context.Tags.FirstOrDefaultAsync(c => c.Id.Equals(tagId)));
        return tags;
    }

    public async Task<IActionResult> AddTagAsync(TagCreation tag)
    {
        var tagExist = await _context.Tags.AnyAsync(c => c.Title.ToLower().Equals(tag.Title.ToLower()));
        if (tagExist) return new StatusCodeResult(409);
        var createTag = new Tag
        {
            Id = Guid.NewGuid(),
            Title = tag.Title,
            CreateDate = DateTime.Now
        };
        _context.Tags.Add(createTag);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
    }

    public async Task<IActionResult> UpdateTagAsync(TagUpdate tag)
    {
        var tagExist = await _context.Tags.FirstOrDefaultAsync(c => c.Id.Equals(tag.Id));
        if (tagExist == null) return new StatusCodeResult(409);
        var updateTag = await _context.Tags.FirstOrDefaultAsync(c => c.Id.Equals(tag.Id));
        updateTag.Title = tag.Title;
        _context.Tags.Update(updateTag);
        _context.Entry(updateTag).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return new StatusCodeResult(200);
    }

    public async Task<IActionResult> DeleteTagAsync(Guid id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null) return new StatusCodeResult(404);
        if (await _context.ArtworkTags.AnyAsync(c => c.ArtworkId.Equals(id)) == true)
        {
            var artworkTags = _context.ArtworkTags.Where(c => c.ArtworkId.Equals(id));
            _context.ArtworkTags.RemoveRange(artworkTags);
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(204);
    }
}