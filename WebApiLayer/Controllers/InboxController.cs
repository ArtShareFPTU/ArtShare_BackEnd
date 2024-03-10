using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Inbox;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class InboxController : ControllerBase
{
    private readonly IInboxService _inboxService;

    public InboxController(IInboxService inboxService)
    {
        _inboxService = inboxService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<List<InboxReceiverResponse>>> GetInboxReceiverResponses(Guid id)
    {
        var list =  await _inboxService.GetInboxByReceiverIdAsync(id);
        if (list == null)
        {
            return NotFound();
        }
        return list;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<List<InboxSenderResponse>>> GetInboxSenderResponses(Guid id)
    {
        var list = await _inboxService.GetInboxBySenderIdAsync(id);
        if (list == null)
        {
            return NotFound();
        }
        return list;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<InboxDetailResponse>> GetInboxDetail(Guid id)
    {
        return await _inboxService.GetInboxByIdAsync(id);
    }
    
    [HttpPost]
    public async Task<ActionResult<Inbox>> CreateInbox(Inbox inbox)
    {
        return await _inboxService.CreateInboxAsync(inbox);
    }
    
}