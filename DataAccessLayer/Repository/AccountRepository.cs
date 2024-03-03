using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

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
        _context.Accounts.Remove(account);
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
}