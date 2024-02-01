using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.Service;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<List<Account>> GetAllAccountAsync()
    {
        return await _accountRepository.GetAllAccountAsync();
    }

    public async Task<Account> GetAccountByIdAsync(Guid id)
    {
        return await _accountRepository.GetAccountByIdAsync(id);
    }

    public async Task AddAccountAsync(Account account)
    {
        await _accountRepository.AddAccountAsync(account);
    }

    public async Task UpdateAccountAsync(Account account)
    {
        await _accountRepository.UpdateAccountAsync(account);
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        await _accountRepository.DeleteAccountAsync(id);
    }
}