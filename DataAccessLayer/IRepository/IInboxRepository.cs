using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IInboxRepository
{
    public Task<List<Inbox>> GetInboxByReceiverIdAsync(Guid id);
    public Task<List<Inbox>> GetInboxBySenderIdAsync(Guid id);

    public Task<Inbox> GetInboxByIdAsync(Guid id);

    public Task<Inbox> CreateInboxAsync(Inbox item);
}