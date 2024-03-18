using AutoMapper;
using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Pagination;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Response.Account;
using ModelLayer.DTOS.Response.Commons;
using ModelLayer.Enum;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace BusinessLogicLayer.Service;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, IConfiguration configuration, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<List<AccountResponse>> GetAllAccountAsync()
    {
        var response = await _accountRepository.GetAllAccountAsync();
        return _mapper.Map<List<AccountResponse>>(response);
    }
    public async Task<Pagination<AccountResponse>> GetAllAccountPagination(int pageindex = 0)
    {
        var response = await _accountRepository.ToPagination(pageindex);
        return _mapper.Map<Pagination<AccountResponse>>(response);
    }
    public async Task<Account> GetAccountByArtworkId(Guid id)
    {
        return await _accountRepository.GetAccountByArtworkId(id);
    }

    public async Task<AccountResponse> GetAccountById(Guid id)
    {
        var response = await _accountRepository.GetAccountById(id);
        return _mapper.Map<AccountResponse>(response);
    }

    public async Task<Guid> GetIdByUserName(string UserName)
    {
        var response = await _accountRepository.GetByUserName(UserName);
        return response.Id;
    }

    public async Task AddAccountAsync(Account account)
    {
        await _accountRepository.AddAccountAsync(account);
    }

    public async Task<ServiceResponse<AccountResponse>> UpdateAccount(UpdateAccountRequest account)
    {
        var respone = new ServiceResponse<AccountResponse>();
        var checkid = await _accountRepository.GetAccountById(account.Id);
        if (checkid == null)
        {
            respone.Success = false;
            respone.Message = "This account does not exist";
            return respone;
        }
        else
        {
            // use Mapper ` request => DB
            var data = _mapper.Map(account, checkid);
            var setdata = await _accountRepository.UpdateAccount(account);
            respone.Success = true;
            respone.Message = "Update successfully";
            respone.Data = _mapper.Map<AccountResponse>(setdata);
        }

        return respone;
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        await _accountRepository.DeleteAccountAsync(id);
    }

    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _accountRepository.GetAccountByEmail(email);
        var admin = await _accountRepository.GetAdminAccount(email, password);
        if (user == null && admin == null)
        {
            response.Success = false;
            response.Message = "User not found!";
        }
        else if (admin != null)
        {
            response.Success = true;
            response.Message = "Admin login Successfully";
            response.Data = CreateTokenForAdmin(admin);
        }
        else if (user.Password != password)
        {
            response.Success = false;
            response.Message = "Wrong password!";
        }
        else if (user.Status == AccountStatus.Inactive.ToString())
        {
            response.Success = false;
            response.Message = "User is inactive!";
        }
        else
        {
            response.Success = true;
            response.Message = "Login Successfully";
            response.Data = CreateToken(user);
        }

        return response;
    }

    public string CreateToken(Account ua)
    {
        var claims = new List<Claim>
            {
                new Claim("Id", ua.Id.ToString()),
                new Claim("Username", ua.UserName.ToString()),
                new Claim(ClaimTypes.Role, "User")
                
            };
        if (ua.Avatar != null)
        {
            claims = new List<Claim>
            {
                new Claim("Id", ua.Id.ToString()),
                new Claim("Username", ua.UserName.ToString()),
                new Claim("Avatar", ua.Avatar.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };
        }

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
            _configuration["Tokens:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string CreateTokenForAdmin(string email)
    {
        var claims = new List<Claim>()
        {
            new Claim("Email", email, SecurityAlgorithms.HmacSha256),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
            _configuration["Tokens:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<ServiceResponse<AccountResponse>> CreateNewAccount(CreateAccountRequest userAccount)
    {
        var respone = new ServiceResponse<AccountResponse>();
        var checkid = await _accountRepository.isExistedByMail(userAccount.Email);
        if (checkid != null)
        {
            respone.Success = false;
            respone.Message = "Email is existed";
            return respone;
        }

        var checkUsername = await _accountRepository.GetByUserName(userAccount.UserName);
        if (checkUsername is not null)
        {
            respone.Success = false;
            respone.Message = "Username is existed";
        }
        else
        {
            var request = _mapper.Map<Account>(userAccount);
            request.CreateDate = DateTime.Now;
            request.Status = AccountStatus.Active.ToString();
            var newaccount = await _accountRepository.CreateAccount(request);
            respone.Data = _mapper.Map<AccountResponse>(newaccount);
            respone.Success = true;
            respone.Message = "Created successfully";
        }

        return respone;
    }

    public async Task<List<Account>> GetTop5AccountsNumArtwork()
    {
        return await _accountRepository.GetTop5AccountsNumArtwork();
    }
}