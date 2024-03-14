using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Account;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IAccountRepository
{
    Task<List<Account>> GetAllAccountAsync();
    Task<Account> GetAccountById(Guid id);
    Task AddAccountAsync(Account account);
    Task<Account> UpdateAccount(UpdateAccountRequest account);
    Task DeleteAccountAsync(Guid id);
    Task<Account> GetAccountByArtworkId(Guid id);
    Task<Account> GetAccountByEmail(string email);
    Task<Account> CreateAccount(Account userAccount);
    Task<Account> isExistedByMail(string email);
    Task<Account> GetByUserName(string username);
    Task<string> GetAdminAccount(string email, string password);
    Task<List<Account>> GetTop5AccountsNumArtwork();
}