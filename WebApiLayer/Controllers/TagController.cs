using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // GET: api/Tag
    [HttpGet]
    public async Task<ActionResult<List<Tag>>> GetTags()
    {
        return await _tagService.GetAllTagAsync();
    }

    /*// GET: api/Tag/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Tag>> GetTag(Guid id)
    {
      if (_context.Tags == null)
      {
          return NotFound();
      }
        var tag = await _context.Tags.FindAsync(id);

        if (tag == null)
        {
            return NotFound();
        }

        return tag;
    }

    // PUT: api/Tag/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTag(Guid id, Tag tag)
    {
        if (id != tag.Id)
        {
            return BadRequest();
        }

        _context.Entry(tag).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TagExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Tag
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Tag>> PostTag(Tag tag)
    {
      if (_context.Tags == null)
      {
          return Problem("Entity set 'ArtShareContext.Tags'  is null.");
      }
        _context.Tags.Add(tag);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (TagExists(tag.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
    }

    // DELETE: api/Tag/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        if (_context.Tags == null)
        {
            return NotFound();
        }
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null)
        {
            return NotFound();
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TagExists(Guid id)
    {
        return (_context.Tags?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}