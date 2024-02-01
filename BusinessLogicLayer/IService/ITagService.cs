using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.IService;

public interface ITagService
{
    Task<List<Tag>> GetAllTagAsync();
    Task<Tag> GetTagByIdAsync(Guid id);
    Task AddTagAsync(Tag Tag);
    Task UpdateTagAsync(Tag Tag);
    Task DeleteTagAsync(Guid id);
}