using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllCommentAsync();
    Task<Comment> GetCommentByIdAsync(Guid id);
    Task AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Guid id);
}