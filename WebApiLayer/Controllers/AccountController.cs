using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Response.Account;
using ModelLayer.DTOS.Response.Commons;
using ModelLayer.DTOS.Validators;

namespace WebApiLayer.Controllers;
[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly UserLoginResponseValidator _loginValidations = new();
    private readonly IHttpContextAccessor _contextAccessor;

    public AccountController(IAccountService accountService, IHttpContextAccessor contextAccessor)
    {
        _accountService = accountService;
        _contextAccessor = contextAccessor;
    }

    // GET: api/Account
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<AccountResponse>>> GetAccount()
    {
        return await _accountService.GetAllAccountAsync();
    }

    // GET: api/Account/5
    //[EnableQuery]
    //[HttpGet("{id}")]
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountResponse>> GetAccount(Guid id)
    {
        var account = await _accountService.GetAccountById(id);
        if (account == null) return NotFound();
        return Ok(account);
    }

    [HttpGet("{artworkId}")]
    public async Task<ActionResult<Account>> GetAccountByArtworkId(Guid artworkId)
    {
        return await _accountService.GetAccountByArtworkId(artworkId);
    }
    
    [HttpGet("{username}")]
    public async Task<ActionResult<Guid>> GetAccountByUserName(string username)
    {
        return await _accountService.GetIdByUserName(username);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ServiceResponse<string>> Login(LoginAccountResponse lg)
    {
        var validationResult = await _loginValidations.ValidateAsync(lg);
        if (!validationResult.IsValid)
        {
            var errorrMessages = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            var response = new ServiceResponse<string>
            {
                /*Success = false,*/
                Message = "Invalid input data." + string.Join("; ", errorrMessages),
                Data = null
            };
            return response;
        }

        var result = await _accountService.Login(lg.Email, lg.Password);
        return result;
    }

    // PUT: api/Account/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ServiceResponse<AccountResponse>> UpdateAccount([FromBody] UpdateAccountRequest account, Guid id)
    {
        var response = await _accountService.UpdateAccount(id, account);
        return response;
    }

    // POST: api/Account
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [AllowAnonymous]
    [HttpPost]
    public async Task<ServiceResponse<AccountResponse>> CreateAccount(CreateAccountRequest account)
    {
        var response = await _accountService.CreateNewAccount(account);
        return response;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<AccountResponse>> GetCurrentAccount()
    {
        var customer = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type.Contains("Id")).Value;
        if (customer == null) return StatusCode(StatusCodes.Status401Unauthorized);
        else return await _accountService.GetAccountById(Guid.Parse(customer));
    }
    [Authorize(Roles = "Admin")]
    // DELETE: api/Account/5
    [HttpDelete("{id}")]
    public async Task DeleteAccount(Guid id)
    {
        await _accountService.DeleteAccountAsync(id);
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task UnBlockAccount(Guid id)
    {
        await _accountService.UnblockAccountAsync(id);
    }
}