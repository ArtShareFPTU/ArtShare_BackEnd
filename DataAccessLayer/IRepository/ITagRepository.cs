using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTagAsync();
    Task<Tag> GetTagByIdAsync(Guid id);
    Task AddTagAsync(Tag tag);
    Task UpdateTagAsync(Tag tag);
    Task DeleteTagAsync(Guid id);
}