using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.IService;

public interface ICommentService
{
    Task<List<Comment>> GetAllCommentAsync();
    Task<Comment> GetCommentByIdAsync(Guid id);
    Task AddCommentAsync(Comment Comment);
    Task UpdateCommentAsync(Comment Comment);
    Task DeleteCommentAsync(Guid id);
}