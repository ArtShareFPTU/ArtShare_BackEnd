using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Category;
using ModelLayer.DTOS.Request.Comment;

namespace BusinessLogicLayer.IService;

public interface ICommentService
{
    Task<List<Comment>> GetAllCommentAsync();
    Task<Comment> GetCommentByIdAsync(Guid id);
    Task<IActionResult> AddCommentAsync(CommentCreation Comment);
    Task UpdateCommentAsync(Comment Comment);
    Task DeleteCommentAsync(Guid id);
}