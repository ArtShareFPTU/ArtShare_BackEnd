using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    // GET: api/Follow
    [HttpGet]
    public async Task<ActionResult<List<Follow>>> GetFollows()
    {
        return await _followService.GetAllFollowAsync();
    }

    /*// GET: api/Follow/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Follow>> GetFollow(Guid id)
    {
      if (_context.Follows == null)
      {
          return NotFound();
      }
        var follow = await _context.Follows.FindAsync(id);

        if (follow == null)
        {
            return NotFound();
        }

        return follow;
    }

    // PUT: api/Follow/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFollow(Guid id, Follow follow)
    {
        if (id != follow.Id)
        {
            return BadRequest();
        }

        _context.Entry(follow).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FollowExists(id))
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

    // POST: api/Follow
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Follow>> PostFollow(Follow follow)
    {
      if (_context.Follows == null)
      {
          return Problem("Entity set 'ArtShareContext.Follows'  is null.");
      }
        _context.Follows.Add(follow);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (FollowExists(follow.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetFollow", new { id = follow.Id }, follow);
    }

    // DELETE: api/Follow/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFollow(Guid id)
    {
        if (_context.Follows == null)
        {
            return NotFound();
        }
        var follow = await _context.Follows.FindAsync(id);
        if (follow == null)
        {
            return NotFound();
        }

        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FollowExists(Guid id)
    {
        return (_context.Follows?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}