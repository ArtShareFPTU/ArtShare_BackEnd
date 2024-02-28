using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Commons;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BusinessLogicLayer.Service;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public AccountService(IAccountRepository accountRepository, IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _configuration = configuration;
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

    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _accountRepository.GetAccountByEmail(email);
        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found";
        }
        else if (user.Password != password)
        {
            response.Success = false;
            response.Message = "Wrong pass";
        }
        else
        {
            response.Data = CreateToken(user);
        }
        return response;
    }
    public string CreateToken(Account ua)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ua.Id.ToString()),
            };

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
            _configuration["Tokens:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}