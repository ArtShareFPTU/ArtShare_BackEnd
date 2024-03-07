using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Comment;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // GET: api/Comment
    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetComments()
    {
        return await _commentService.GetAllCommentAsync();
    }

    //POST: api/Comment
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("create")]
    public async Task<ActionResult> PostComment(CommentCreation comment)
    {
        try
        {
           await _commentService.AddCommentAsync(comment);

            return Ok();
        }
        catch (Exception e)
        {

            return BadRequest(e);
        }
    }
    /*// GET: api/Comment/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(Guid id)
    {
      if (_context.Comments == null)
      {
          return NotFound();
      }
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return comment;
    }

    // PUT: api/Comment/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, Comment comment)
    {
        if (id != comment.Id)
        {
            return BadRequest();
        }

        _context.Entry(comment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(id))
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

     POST: api/Comment
     To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Comment>> PostComment(Comment comment)
    {
      if (_context.Comments == null)
      {
          return Problem("Entity set 'ArtShareContext.Comments'  is null.");
      }
        _context.Comments.Add(comment);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (CommentExists(comment.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
    }

    // DELETE: api/Comment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        if (_context.Comments == null)
        {
            return NotFound();
        }
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(Guid id)
    {
        return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}