using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using ModelLayer.DTOS.Response;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<ArtworkRespone>> GetArtworkById(Guid id)
    {
        return await _artworkService.GetArtworkByIdAsync(id);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateArtwork([FromForm] ArtworkCreation artworkCreation)
    {
        try
        {
            var result = await _artworkService.AddArtworkAsync(artworkCreation);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409)
                    return StatusCode(StatusCodes.Status409Conflict, "This artwork is existed");
                else if (statusCodeResult.StatusCode == 201)
                    return StatusCode(StatusCodes.Status201Created, "Artwork create success");
                else if (statusCodeResult.StatusCode == 500)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Image not found or error when uploading");
                else if (statusCodeResult.StatusCode == 415)
                    return StatusCode(StatusCodes.Status415UnsupportedMediaType, "File type is not supported");
            }

            return BadRequest("Error when creating artwork");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateArtwork([FromForm]ArtworkUpdate artworkUpdate)
    {
        try
        {
            var result = await _artworkService.UpdateArtworkAsync(artworkUpdate);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409)
                    return StatusCode(StatusCodes.Status409Conflict, "This artwork was removed or not existed before");
                else if (statusCodeResult.StatusCode == 200)
                    return StatusCode(StatusCodes.Status200OK, "Artwork update success");
                else if (statusCodeResult.StatusCode == 500)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error when uploading image");
                else if (statusCodeResult.StatusCode == 415)
                    return StatusCode(StatusCodes.Status415UnsupportedMediaType, "Artwork update success");
            }

            return BadRequest("Error when updating artwork");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("remove/{id}")]
    public async Task<IActionResult> RemoveArtwork(Guid id)
    {
        try
        {
            var result = await _artworkService.DeleteArtworkAsync(id);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 404)
                    return StatusCode(StatusCodes.Status409Conflict, "This artwork was disabled or not existed before");
                else if (statusCodeResult.StatusCode == 200)
                    return StatusCode(StatusCodes.Status200OK, "Artwork disabled success");
            }

            return BadRequest("Error when removing artwork");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("createTag")]
    public async Task<IActionResult> CreateTag4Artwork([FromForm]ArtworkTagAddition artworkTagAddition)
    {
        try
        {
            var result = await _artworkService.AddTag4Artwork(artworkTagAddition);
            if(result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) return StatusCode(StatusCodes.Status409Conflict, "This tag was removed or was added before");
                else if (statusCodeResult.StatusCode == 201) return StatusCode(StatusCodes.Status201Created, "Add tag for artwork success");
            }
            return BadRequest("Error when adding tag for artwork");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("createCategory")]
    public async Task<IActionResult> CreateCategory4Artwork([FromForm] ArtworkCategoryAddition artworkCategoryAddition)
    {
        try
        {
            var result = await _artworkService.AddCategory4Artwork(artworkCategoryAddition);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) return StatusCode(StatusCodes.Status409Conflict, "This category was removed or was added before");
                else if (statusCodeResult.StatusCode == 201) return StatusCode(StatusCodes.Status201Created, "Add category for artwork success");
            }
            return BadRequest("Error when adding tag for artwork");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("updateTag")]
    public async Task<IActionResult> UpdateTag4Artwork([FromForm] ArtworkTagUpdate artworkTagUpdate)
    {
        try
        {
            var result = await _artworkService.UpdateTag4Artwork(artworkTagUpdate);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) return StatusCode(StatusCodes.Status409Conflict, "This tag was removed before");
                else if (statusCodeResult.StatusCode == 200) return StatusCode(StatusCodes.Status200OK, "Update tag for artwork success");
            }
            return BadRequest("Update tag for artwork failed");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("updateCategory")]
    public async Task<IActionResult> UpdateCategory4Artwork([FromForm] ArtworkCategoryUpdate artworkCategoryUpdate)
    {
        try
        {
            var result = await _artworkService.UpdateCategory4Artwork(artworkCategoryUpdate);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) return StatusCode(StatusCodes.Status409Conflict, "This category was removed before");
                else if (statusCodeResult.StatusCode == 200) return StatusCode(StatusCodes.Status200OK, "Update tag for artwork success");
            }
            return BadRequest("Update category for artwork failed");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("removeTag")]
    public async Task<IActionResult> RemoveTag4Artwork(Guid id)
    {
        try
        {
            var result = await _artworkService.RemoveTag4Artwork(id);
            if(result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 404) return StatusCode(StatusCodes.Status404NotFound, "This tag is not found on the artwork");
                else if (statusCodeResult.StatusCode == 204) return StatusCode(StatusCodes.Status204NoContent, "Remove tag for artwork success");
            }
            return BadRequest("Error when removing tag for artwork");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("removeCategory")]
    public async Task<IActionResult> RemoveCategory4Artwork(Guid id)
    {
        try
        {
            var result = await _artworkService.RemoveCategory4Artwork(id);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 404) return StatusCode(StatusCodes.Status404NotFound, "This category is not found on the artwork");
                else if (statusCodeResult.StatusCode == 204) return StatusCode(StatusCodes.Status204NoContent, "Remove category for artwork success");
            }
            return BadRequest("Error when removing category for artwork");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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