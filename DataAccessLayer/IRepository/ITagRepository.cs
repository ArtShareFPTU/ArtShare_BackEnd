using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Tags;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTagAsync();
    Task<Tag> GetTagByIdAsync(Guid id);
    Task<IActionResult> AddTagAsync(TagCreation tag);
    Task<IActionResult> UpdateTagAsync(TagUpdate tag);
    Task<IActionResult> DeleteTagAsync(Guid id);
}