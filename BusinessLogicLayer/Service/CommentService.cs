using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Comment;

namespace BusinessLogicLayer.Service;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _CommentRepository;

    public CommentService(ICommentRepository CommentRepository)
    {
        _CommentRepository = CommentRepository;
    }

    public async Task<List<Comment>> GetAllCommentAsync()
    {
        return await _CommentRepository.GetAllCommentAsync();
    }

    public async Task<Comment> GetCommentByIdAsync(Guid id)
    {
        return await _CommentRepository.GetCommentByIdAsync(id);
    }

    public async Task<IActionResult> AddCommentAsync(CommentCreation comment)
    {
        return await _CommentRepository.AddCommentAsync(comment);
    }

    public async Task UpdateCommentAsync(Comment Comment)
    {
        await _CommentRepository.UpdateCommentAsync(Comment);
    }

    public async Task DeleteCommentAsync(Guid id)
    {
        await _CommentRepository.DeleteCommentAsync(id);
    }
}