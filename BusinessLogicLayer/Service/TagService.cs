using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Tags;

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

    public async Task<IActionResult> AddTagAsync(TagCreation Tag)
    {
        return await _TagRepository.AddTagAsync(Tag);
    }
    public async Task<List<Tag>> GetTagByArtworkIdAsync(Guid id)
    {
        return await _TagRepository.GetTagByArtworkIdAsync(id);
    }

    public async Task<IActionResult> UpdateTagAsync(TagUpdate Tag)
    {
        return await _TagRepository.UpdateTagAsync(Tag);
    }

    public async Task<IActionResult> DeleteTagAsync(Guid id)
    {
        return await _TagRepository.DeleteTagAsync(id);
    }
}