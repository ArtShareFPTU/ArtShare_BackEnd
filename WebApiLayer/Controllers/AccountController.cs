using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Response.Commons;
using ModelLayer.DTOS.Validators;

namespace WebApiLayer.Controllers;
[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly UserLoginResponseValidator _loginValidations;
    public AccountController(IAccountService accountService, UserLoginResponseValidator loginValidations)
    {
        _accountService = accountService;
        _loginValidations = loginValidations;
    }

    // GET: api/Account
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<List<Account>>> GetAccount()
    {

        return await _accountService.GetAllAccountAsync();
    }

    // GET: api/Account/5
    //[EnableQuery]
    //[HttpGet("{id}")]
    [HttpGet("odata/Account({id})")]

    public async Task<ActionResult<Account>> GetAccount(Guid id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
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
                Success = false,
                Message = "Invalid input data." + string.Join("; ", errorrMessages),
                Data = null
            };
            return response;
        }
        var result = await _accountService.Login(lg.Email, lg.Password);
        return result;
    }
    /*// PUT: api/Account/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAccount(Guid id, Account account)
    {
        if (id != account.Id)
        {
            return BadRequest();
        }

        _context.Entry(account).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AccountExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Account
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Account>> PostAccount(Account account)
    {
        if (_context.Accounts == null)
        {
            return Problem("Entity set 'ArtShareContext.Accounts'  is null.");
        }

        _context.Accounts.Add(account);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (AccountExists(account.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetAccount", new { id = account.Id }, account);
    }

    // DELETE: api/Account/5
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
    }

    private bool AccountExists(Guid id)
    {
        return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}