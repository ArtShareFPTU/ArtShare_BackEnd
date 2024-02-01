using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ArtworkController : ControllerBase
{
    private readonly IArtworkService _artworkService;

    public ArtworkController(IArtworkService artworkService)
    {
        _artworkService = artworkService;
    }

    // GET: api/Artwork
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artwork>>> GetArtworks()
    {
        return await _artworkService.GetAllArtworkAsync();
    }

    /*// GET: api/Artwork/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Artwork>> GetArtwork(Guid id)
    {
      if (_context.Artworks == null)
      {
          return NotFound();
      }
        var artwork = await _context.Artworks.FindAsync(id);

        if (artwork == null)
        {
            return NotFound();
        }

        return artwork;
    }

    // PUT: api/Artwork/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutArtwork(Guid id, Artwork artwork)
    {
        if (id != artwork.Id)
        {
            return BadRequest();
        }

        _context.Entry(artwork).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ArtworkExists(id))
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

    // POST: api/Artwork
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Artwork>> PostArtwork(Artwork artwork)
    {
      if (_context.Artworks == null)
      {
          return Problem("Entity set 'ArtShareContext.Artworks'  is null.");
      }
        _context.Artworks.Add(artwork);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (ArtworkExists(artwork.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetArtwork", new { id = artwork.Id }, artwork);
    }

    // DELETE: api/Artwork/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtwork(Guid id)
    {
        if (_context.Artworks == null)
        {
            return NotFound();
        }
        var artwork = await _context.Artworks.FindAsync(id);
        if (artwork == null)
        {
            return NotFound();
        }

        _context.Artworks.Remove(artwork);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ArtworkExists(Guid id)
    {
        return (_context.Artworks?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}