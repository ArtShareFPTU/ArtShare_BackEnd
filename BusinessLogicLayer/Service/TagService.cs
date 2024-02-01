using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.Service;

public class TagService : ITagService
{
    private readonly ITagRepository _TagRepository;

    public TagService(ITagRepository TagRepository)
    {
        _TagRepository = TagRepository;
    }

    public async Task<List<Tag>> GetAllTagAsync()
    {
        return await _TagRepository.GetAllTagAsync();
    }

    public async Task<Tag> GetTagByIdAsync(Guid id)
    {
        return await _TagRepository.GetTagByIdAsync(id);
    }

    public async Task AddTagAsync(Tag Tag)
    {
        await _TagRepository.AddTagAsync(Tag);
    }

    public async Task UpdateTagAsync(Tag Tag)
    {
        await _TagRepository.UpdateTagAsync(Tag);
    }

    public async Task DeleteTagAsync(Guid id)
    {
        await _TagRepository.DeleteTagAsync(id);
    }
}