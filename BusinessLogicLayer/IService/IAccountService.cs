using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Response.Account;
using ModelLayer.DTOS.Response.Commons;

namespace BusinessLogicLayer.IService;

public interface IAccountService
{
    Task<List<AccountResponse>> GetAllAccountAsync();
    Task<AccountResponse> GetAccountById(Guid id);
    Task<Guid> GetIdByUserName(string UserName);
    Task AddAccountAsync(Account account);
    Task<Account> GetAccountByArtworkId(Guid id);
    Task<ServiceResponse<AccountResponse>> UpdateAccount(Guid id, UpdateAccountRequest account);
    Task DeleteAccountAsync(Guid id);
    Task<ServiceResponse<string>> Login(string email, string password);
    Task<ServiceResponse<AccountResponse>> CreateNewAccount(CreateAccountRequest userAccount);
}