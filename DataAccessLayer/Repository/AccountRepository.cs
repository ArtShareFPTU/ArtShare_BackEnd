using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer.BussinessObject;
using ModelLayer.Enum;

namespace DataAccessLayer.BussinessObject.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly ArtShareContext _context;

    public AccountRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Account>> GetAllAccountAsync()
    {
        return await _context.Accounts.Include(c => c.Artworks).Include(c => c.FollowFollowers)
            .Include(c => c.FollowArtists).ToListAsync();
    }

    public async Task<Account> GetAccountById(Guid id)
    {
        var account = _context.Set<Account>().Include(c => c.Artworks).Include(c => c.FollowFollowers)
            .Include(c => c.FollowArtists).FirstOrDefault(c => c.Id.Equals(id));
        return account;
    }

    public async Task<Account> GetAccountByArtworkId(Guid id)
    {
        var artwork = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(id));
        return await _context.Accounts.FirstOrDefaultAsync(c => c.Id.Equals(artwork.AccountId));
    }

    public async Task AddAccountAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }

    public async Task<Account> UpdateAccount(Account account)
    {
        _context.Accounts.Update(account);
        _context.SaveChanges();
        return account;
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        var account = await _context.Accounts.FindAsync(id);
        account.Status = AccountStatus.Inactive.ToString();
        await _context.SaveChangesAsync();
    }

    public async Task UnblockAccountAsync(Guid id)
    {
        var account = await _context.Accounts.FindAsync(id);
        account.Status = AccountStatus.Active.ToString();
        await _context.SaveChangesAsync();
    }

    public async Task<Account> GetAccountByEmail(string email)
    {
        return await _context.Set<Account>().FirstOrDefaultAsync(c => c.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<Account> isExistedByMail(string email)
    {
        var account = await _context.Set<Account>().FirstOrDefaultAsync(c => c.Email == email);
        return account;
    }

    public async Task<Account> CreateAccount(Account userAccount)
    {
        await _context.Accounts.AddAsync(userAccount);
        _context.SaveChanges();
        return userAccount;
    }

    public async Task<Account> GetByUserName(string username)
    {
        var account = await _context.Set<Account>()
            .FirstOrDefaultAsync(c => c.UserName.ToLower().Equals(username.ToLower()));
        return account;
    }

    public async Task<string> GetAdminAccount(string email, string password)
    {
        IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

        // Check if the configuration key exists
        if (config.GetSection("AdminAccount").Exists())
        {
            string emailJson = config["AdminAccount:adminemail"];
            string passwordJson = config["AdminAccount:adminpassword"];

            // Check if both email and password match
            if (emailJson == email && passwordJson == password)
            {
                return emailJson;
            }
        }

        return null;
    }
}