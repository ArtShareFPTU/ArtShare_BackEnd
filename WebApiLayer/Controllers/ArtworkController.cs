using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using ModelLayer.DTOS.Response;

namespace WebApiLayer.Controllers;
[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class ArtworkController : ControllerBase
{
    private readonly IArtworkService _artworkService;
    private readonly IHttpContextAccessor _contextAccessor;

    public ArtworkController(IArtworkService artworkService, IHttpContextAccessor contextAccessor)
    {
        _artworkService = artworkService;
        _contextAccessor = contextAccessor;
    }

    // GET: api/Artwork
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artwork>>> GetArtworks()
    {
        return await _artworkService.GetAllArtworkAsync();
    }
	[AllowAnonymous]
	[HttpGet("{id}")]
    public async Task<ActionResult<ArtworkRespone>> GetArtworkById(Guid id)
    {
        return await _artworkService.GetArtworkByIdAsync(id);
    }
    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<ArtworkRespone>> GetOwnArtworks()
    {
        var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
        return await _artworkService.GetArtworkByArtistId(Guid.Parse(customer));
    }
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateArtwork([FromForm] ArtworkCreation artworkCreation)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if(customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateArtwork([FromForm]ArtworkUpdate artworkUpdate)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPost("remove/{id}")]
    public async Task<IActionResult> RemoveArtwork(Guid id)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPost("createTag")]
    public async Task<IActionResult> CreateTag4Artwork([FromForm]ArtworkTagAddition artworkTagAddition)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPost("createCategory")]
    public async Task<IActionResult> CreateCategory4Artwork([FromForm] ArtworkCategoryAddition artworkCategoryAddition)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPut("updateTag")]
    public async Task<IActionResult> UpdateTag4Artwork([FromForm] ArtworkTagUpdate artworkTagUpdate)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPut("updateCategory")]
    public async Task<IActionResult> UpdateCategory4Artwork([FromForm] ArtworkCategoryUpdate artworkCategoryUpdate)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPost("removeTag")]
    public async Task<IActionResult> RemoveTag4Artwork(Guid id)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [Authorize]
    [HttpPost("removeCategory")]
    public async Task<IActionResult> RemoveCategory4Artwork(Guid id)
    {
        try
        {
            var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
            if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
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
    [HttpGet]
    public async Task<List<ArtworkCategory>> GetArtworkCategories() => await _artworkService.GetArtworkCategories();
    [HttpGet]
    public async Task<List<ArtworkCategory>> GetArtworkCategoriesByArtworkId(Guid id) => await _artworkService.GetArtworkCategoryByArtworkId(id);
    [HttpGet]
    public async Task<List<ArtworkCategory>> GetArtworkCategoriesByCategoryId(Guid id) => await _artworkService.GetArtworkCategoryByCategoryId(id);
    [HttpGet]
    public async Task<List<ArtworkTag>> GetArtworkTags() => await _artworkService.GetArtworkTags();
    [HttpGet]
    public async Task<List<ArtworkTag>> GetArtworkTagsByArtworkId(Guid id) => await _artworkService.GetArtworkTagByArtworkId(id);
    [HttpGet]
    public async Task<List<ArtworkTag>> GetArtworkTagsByTagId(Guid id) => await _artworkService.GetArtworkTagByTagId(id);
    [AllowAnonymous]
    [HttpGet("resource")]
    public async Task<ActionResult<List<Artwork>>> GetArtworkFromSearch([FromQuery] string search)
    {
        return await _artworkService.GetArtworkFromSearch(search);
    }
    [AllowAnonymous]
	[HttpGet("{id}")]
	public async Task<ActionResult<List<ArtworkRespone>>> GetArtworksByArtistId(Guid id)
	{
		return await _artworkService.GetArtworkByArtistId(id);
	}
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> DownloadImage(Guid id)
    {
        var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
        if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
        var list = await _artworkService.GetAllArtworkAsync();
        var url = list.FirstOrDefault(c => c.Id.Equals(id)).PremiumUrl;
        if (!url.StartsWith("https://i.ibb.co/") || url == null || url.Length == 0)
        {
            return BadRequest("Invalid image URL");
        }

        using (var httpClient = new HttpClient())
        {
            var imageData = await httpClient.GetByteArrayAsync(url);
            var fileName = Path.GetFileName(new Uri(url).LocalPath);
            return File(imageData, "image/jpg", fileName);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> DownloadReduceImage(Guid id)
    {
        var list = await _artworkService.GetAllArtworkAsync();
        var url = list.FirstOrDefault(c => c.Id.Equals(id)).Url;
        if (!url.StartsWith("https://i.ibb.co/") || url == null || url.Length == 0)
        {
            return BadRequest("Invalid image URL");
        }

        using (var httpClient = new HttpClient())
        {
            var imageData = await httpClient.GetByteArrayAsync(url);
            var fileName = Path.GetFileName(new Uri(url).LocalPath);
            return File(imageData, "image/jpg", fileName);
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