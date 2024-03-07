using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;
using ModelLayer.DTOS.Request.Comment;

namespace DataAccessLayer.BussinessObject.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ArtShareContext _context;

    public CommentRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Comment>> GetAllCommentAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment> GetCommentByIdAsync(Guid id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<IActionResult> AddCommentAsync(CommentCreation comment)
    {
        var commentToAdd = new Comment
        {
            Id = Guid.NewGuid(),
            AccountId = comment.AccountId,
            ArtworkId = comment.ArtworkId,
            Content = comment.Content,
            CreateDate = DateTime.Now
        };
        _context.Comments.Add(commentToAdd);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);

    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _context.Entry(comment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id);
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}