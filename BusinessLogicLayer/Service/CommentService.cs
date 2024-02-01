using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

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

    public async Task AddCommentAsync(Comment Comment)
    {
        await _CommentRepository.AddCommentAsync(Comment);
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