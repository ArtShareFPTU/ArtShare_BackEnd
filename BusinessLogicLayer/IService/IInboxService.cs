using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Inbox;

namespace BusinessLogicLayer.IService;

public interface IInboxService
{
    Task<List<InboxReceiverResponse>> GetInboxByReceiverIdAsync(Guid id);
    Task<List<InboxSenderResponse>> GetInboxBySenderIdAsync(Guid id);
    Task<InboxDetailResponse> GetInboxByIdAsync(Guid id);
    Task<Inbox> CreateInboxAsync(Inbox item);
}