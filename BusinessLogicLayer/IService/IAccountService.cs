using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Commons;

namespace BusinessLogicLayer.IService;

public interface IAccountService
{
    Task<List<Account>> GetAllAccountAsync();
    Task<Account> GetAccountByIdAsync(Guid id);
    Task AddAccountAsync(Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(Guid id);
    Task<ServiceResponse<string>> Login(string email, string password);
}