using BusinessLogicLayer.IService;
using BusinessLogicLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;
using ModelLayer.DTOS.Request.Tags;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<Tag>> GetTagById (Guid id) => await _tagService.GetTagByIdAsync(id);

    [HttpPost("create")]
    public async Task<IActionResult> CreateTag(TagCreation tagCreation)
    {
        try
        {
            var result = await _tagService.AddTagAsync(tagCreation);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) { return StatusCode(StatusCodes.Status409Conflict, "This tag is existed"); }
                else if (statusCodeResult.StatusCode == 201) { return StatusCode(StatusCodes.Status201Created, "Tag create success"); }
            }
            return BadRequest("Error when creating tag");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateTag(TagUpdate tagUpdate)
    {
        try
        {
            var result = await _tagService.UpdateTagAsync(tagUpdate);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) { return StatusCode(StatusCodes.Status409Conflict, "This tag was removed or not existed before"); }
                else if (statusCodeResult.StatusCode == 201) { return StatusCode(StatusCodes.Status200OK, "Tag update success"); }
            }
            return BadRequest("Error when updating tag");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveTag(Guid id)
    {
        try
        {
            var result = await _tagService.DeleteTagAsync(id);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 404) { return StatusCode(StatusCodes.Status404NotFound, "This tag was removed or not existed before"); }
                else if (statusCodeResult.StatusCode == 201) { return StatusCode(StatusCodes.Status201Created, "Tag remove success"); }
            }
            return BadRequest("Error when removing tag");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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