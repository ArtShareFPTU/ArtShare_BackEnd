using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.Enum;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System.Text.Json;

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

    public async Task<Account> UpdateAccount(UpdateAccountRequest account)
    {
        var response = await _context.Accounts.FirstOrDefaultAsync(c => c.Id == account.Id);
        if (account.Avatar != null)
        {
            var fileExtension = Path.GetExtension(account.Avatar.FileName)?.ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png"
                && fileExtension != ".bmp" && fileExtension != ".gif" && fileExtension != ".tiff"
                && fileExtension != ".webp" && fileExtension != ".heic" && fileExtension != ".pdf")
                return null;
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await account.Avatar.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }
            var imgbbPremiumURL = await UploadToImgBB(imageData,response.UserName);
            if (imgbbPremiumURL == null) return null;

            // Resize the image using ImageSharp
            using var image = SixLabors.ImageSharp.Image.Load(imageData);
            image.Mutate(x => x.Resize(640, 360));

            response.Avatar = imgbbPremiumURL;
        }
        response.FullName = account.FullName;
        response.Description = account.Description;
        _context.Accounts.Update(response);
        await _context.SaveChangesAsync();
        return response;
    }
    private async Task<string?> UploadToImgBB(byte[] imageData, string title,
        CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        using var form = new MultipartFormDataContent();
        form.Add(new ByteArrayContent(imageData), "image", title);

        var response = await client.PostAsync("https://api.imgbb.com/1/upload?key=ed1d017feac2eabe0a248e4236b28736",
            form, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var byteArray = await response.Content.ReadAsByteArrayAsync();

            using var stream = new MemoryStream(byteArray);
            using var jsonDocument = await JsonDocument.ParseAsync(stream);

            var root = jsonDocument.RootElement;
            if (root.TryGetProperty("data", out var dataElement))
                if (dataElement.TryGetProperty("url", out var urlElement))
                    return urlElement.GetString();
        }

        return null;
    }
    public async Task DeleteAccountAsync(Guid id)
    {
        var account = await _context.Accounts.FindAsync(id);
        account.Status = AccountStatus.Inactive.ToString();
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
    public async Task<List<Account>> GetTop5AccountsNumArtwork()
    {
        return await _context.Accounts.Include(c => c.Artworks).OrderByDescending(c => c.Artworks.Count()).Include(c => c.FollowFollowers)
            .Include(c => c.FollowArtists).Take(5).ToListAsync();
    }
}