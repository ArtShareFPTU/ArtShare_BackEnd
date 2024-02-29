using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IAccountRepository
{
    Task<List<Account>> GetAllAccountAsync();
    Task<Account> GetAccountByIdAsync(Guid id);
    Task AddAccountAsync(Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(Guid id);
    Task<Account> GetAccountByEmail(string email);
}