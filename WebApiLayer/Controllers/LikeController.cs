using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Like;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    // GET: api/Like
    [HttpGet]
    public async Task<ActionResult<List<Like>>> GetLikes()
    {
        return await _likeService.GetAllLikeAsync();
    }

    [HttpGet("{artworkId}")]
    public async Task<ActionResult<List<Like>>> GetLikeByArtworkId(Guid artworkId)
    {
        return await _likeService.GetLikeByArtworkId(artworkId);
    }

    [HttpPost("create")]
    public async Task<ActionResult> addLike(LikeCreation likeCreation)
    {
        try
        {
            await _likeService.AddLikeAsync(likeCreation);
            return Ok();
        }
        catch (Exception e)
        {

            return BadRequest(e);
        }
    }

    [HttpDelete("deleteLike")]
    public async Task<IActionResult> DeleteLike(Guid id, Guid artWorkId)
    {
        try
        {
            await _likeService.DeleteLikeAsync(id, artWorkId);
            return Ok();
        }
        catch (Exception e)
        {

            return BadRequest(e);
        }



    }
    [HttpGet("checkLikeExists")]
    public async Task<IActionResult> CheckLikeExists(Guid accountId, Guid artworkId)
    {
        try
        {
            var likeExists = await _likeService.CheckIfLikeExists(accountId, artworkId);
            return Ok(likeExists);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("getLikeId")]
    public async Task<string> GetLikeId(Guid accountId, Guid artworkId)
    {
            var likeId = await _likeService.GetLikeId(accountId, artworkId);
        return likeId.ToString();
        
    }


    /*// GET: api/Like/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Like>> GetLike(Guid id)
    {
      if (_context.Likes == null)
      {
          return NotFound();
      }
        var like = await _context.Likes.FindAsync(id);

        if (like == null)
        {
            return NotFound();
        }

        return like;
    }

    // PUT: api/Like/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLike(Guid id, Like like)
    {
        if (id != like.Id)
        {
            return BadRequest();
        }

        _context.Entry(like).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LikeExists(id))
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

    // POST: api/Like
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Like>> PostLike(Like like)
    {
      if (_context.Likes == null)
      {
          return Problem("Entity set 'ArtShareContext.Likes'  is null.");
      }
        _context.Likes.Add(like);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (LikeExists(like.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetLike", new { id = like.Id }, like);
    }

    // DELETE: api/Like/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLike(Guid id)
    {
        if (_context.Likes == null)
        {
            return NotFound();
        }
        var like = await _context.Likes.FindAsync(id);
        if (like == null)
        {
            return NotFound();
        }

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LikeExists(Guid id)
    {
        return (_context.Likes?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}