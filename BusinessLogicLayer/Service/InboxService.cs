using AutoMapper;
using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Inbox;

namespace BusinessLogicLayer.Service;

public class InboxService : IInboxService
{
    private readonly IInboxRepository _inboxRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    
    public InboxService(IInboxRepository inboxRepository, IAccountRepository accountRepository ,IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _inboxRepository = inboxRepository;
    }
    
    public async Task<List<InboxReceiverResponse>> GetInboxByReceiverIdAsync(Guid id)
    {
        var result = await _inboxRepository.GetInboxByReceiverIdAsync(id);
        return _mapper.Map<List<InboxReceiverResponse>>(result);
    }

    public async Task<List<InboxSenderResponse>> GetInboxBySenderIdAsync(Guid id)
    {
        var result = await _inboxRepository.GetInboxBySenderIdAsync(id);
        return _mapper.Map<List<InboxSenderResponse>>(result);
    }

    public async Task<InboxDetailResponse> GetInboxByIdAsync(Guid id)
    {
        var result = await _inboxRepository.GetInboxByIdAsync(id);
        return _mapper.Map<InboxDetailResponse>(result);
    }

    public async Task<Inbox> CreateInboxAsync(Inbox item)
    {
        item.Receiver = null;
        var inbox = await _inboxRepository.CreateInboxAsync(item);
        return _mapper.Map<Inbox>(inbox);
    }
}