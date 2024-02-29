using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategorys()
    {
        return await _categoryService.GetAllCategoryAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(Guid id) => await _categoryService.GetCategoryByIdAsync(id);

    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromForm]CategoryCreation categoryCreation)
    {
        try
        {
            var result = await _categoryService.AddCategoryAsync(categoryCreation);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) { return StatusCode(StatusCodes.Status409Conflict, "This category has existed before"); }
                else if (statusCodeResult.StatusCode == 201) { return StatusCode(StatusCodes.Status201Created, "Category add success"); }
            }
            return BadRequest("Error when creating category");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCategory([FromForm]CategoryUpdate categoryUpdate)
    {
        try
        {
            var result = await _categoryService.UpdateCategoryAsync(categoryUpdate);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 409) { return StatusCode(StatusCodes.Status409Conflict, "This category was removed or not created before"); }
                else if (statusCodeResult.StatusCode == 200) { return StatusCode(StatusCodes.Status200OK, "Category update success"); }
            }
            return BadRequest("Error when updating category");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveCategory(Guid id)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result is StatusCodeResult statusCodeResult)
            {
                if (statusCodeResult.StatusCode == 404) { return StatusCode(StatusCodes.Status404NotFound, "This category was removed or not created before"); }
                else if (statusCodeResult.StatusCode == 204) { return StatusCode(StatusCodes.Status204NoContent, "Category remove success"); }
            }
            return BadRequest("Error when removing category");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /*// GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
      if (_context.Categories == null)
      {
          return NotFound();
      }
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    // PUT: api/Category/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(Guid id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
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

    // POST: api/Category
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
      if (_context.Categories == null)
      {
          return Problem("Entity set 'ArtShareContext.Categories'  is null.");
      }
        _context.Categories.Add(category);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (CategoryExists(category.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetCategory", new { id = category.Id }, category);
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        if (_context.Categories == null)
        {
            return NotFound();
        }
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(Guid id)
    {
        return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}