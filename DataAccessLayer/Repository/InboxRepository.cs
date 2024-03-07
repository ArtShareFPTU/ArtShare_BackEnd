using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.Repository;

public class InboxRepository : IInboxRepository
{
    private readonly ArtShareContext _context;

    public InboxRepository()
    {
        _context = new ArtShareContext();
    }
    
    public async Task<List<Inbox>> GetInboxByReceiverIdAsync(Guid id)
    {
        return await _context.Inboxes.Include(i => i.Sender).Where(i => i.ReceiverId == id).ToListAsync();
    }

    public async Task<List<Inbox>> GetInboxBySenderIdAsync(Guid id)
    {
        return await _context.Inboxes.Include(i => i.Receiver).Where(i => i.SenderId == id).ToListAsync();
    }

    public async Task<Inbox> GetInboxByIdAsync(Guid id)
    {
        return await _context.Inboxes.FirstOrDefaultAsync(i => i.Id == id);
    }

    public Task<Inbox> CreateInboxAsync(Inbox item)
    { 
        _context.Inboxes.Add(item);
        _context.SaveChanges();
        return GetInboxByIdAsync(item.Id);
    }
}