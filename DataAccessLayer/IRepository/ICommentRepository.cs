using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;
using ModelLayer.DTOS.Request.Comment;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllCommentAsync();
    Task<Comment> GetCommentByIdAsync(Guid id);
    Task<IActionResult> AddCommentAsync(CommentCreation comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Guid id);
}