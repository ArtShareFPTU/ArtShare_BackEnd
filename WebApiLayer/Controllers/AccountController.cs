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

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // GET: api/Account
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
    [HttpPut]
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

    /*// DELETE: api/Account/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        if (_context.Accounts == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }*/
}