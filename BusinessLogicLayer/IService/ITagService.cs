using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Tags;

namespace BusinessLogicLayer.IService;

public interface ITagService
{
    Task<List<Tag>> GetAllTagAsync();
    Task<Tag> GetTagByIdAsync(Guid id);
    Task<List<Tag>> GetTagByArtworkIdAsync(Guid id);
    Task<IActionResult> AddTagAsync(TagCreation Tag);
    Task<IActionResult> UpdateTagAsync(TagUpdate Tag);
    Task<IActionResult> DeleteTagAsync(Guid id);
}