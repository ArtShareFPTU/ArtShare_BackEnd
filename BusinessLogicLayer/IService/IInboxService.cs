using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Inbox;
using ModelLayer.DTOS.Response.Inbox;

namespace BusinessLogicLayer.IService;

public interface IInboxService
{
    Task<List<InboxReceiverResponse>> GetInboxByReceiverIdAsync(Guid id);
    Task<List<InboxSenderResponse>> GetInboxBySenderIdAsync(Guid id);
    Task<InboxDetailResponse> GetInboxByIdAsync(Guid id);
    Task<IActionResult> CreateInboxAsync(InboxCreation item);
}