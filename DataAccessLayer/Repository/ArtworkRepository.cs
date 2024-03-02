using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLayer.BussinessObject.Repository;

public class ArtworkRepository : IArtworkRepository
{
    private readonly ArtShareContext _context;

    public ArtworkRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<Artwork>> GetAllArtworkAsync()
    {
        return await _context.Artworks.Include(a => a.ArtworkCategories).Include(a => a.ArtworkTags)
            .Include(a => a.Comments).Include(a => a.LikesNavigation).Include(a => a.OrderDetails).ToListAsync();
    }

    public async Task<Artwork> GetArtworkByIdAsync(Guid id)
    {
        return await _context.Artworks.Include(a => a.ArtworkCategories).Include(a => a.ArtworkTags)
            .Include(a => a.Comments).Include(a => a.LikesNavigation).Include(a => a.OrderDetails)
            .FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public async Task<IActionResult> AddArtworkAsync(ArtworkCreation artwork)
    {
        if (artwork.Image == null && artwork.Url == null) return new StatusCodeResult(500);
        if (artwork.Image != null)
        {
            var fileExtension = Path.GetExtension(artwork.Image.FileName)?.ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png"
                && fileExtension != ".bmp" && fileExtension != ".gif" && fileExtension != ".tiff"
                && fileExtension != ".webp" && fileExtension != ".heic" && fileExtension != ".pdf")
                return new StatusCodeResult(415);
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await artwork.Image.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var imgbbURL = await UploadToImgBB(imageData, artwork.Title);
            if (imgbbURL == null && artwork.Url.Length == 0) return new StatusCodeResult(500);
            if (artwork.Url == null || artwork.Url.Length == 0) artwork.Url = imgbbURL;
        }

        var imageExist = await _context.Artworks.FirstOrDefaultAsync(c =>
            c.AccountId.Equals(artwork.AccountId) && c.Title.ToLower().Equals(artwork.Title.ToLower()));
        if (imageExist != null) return new StatusCodeResult(409);

        var createArtwork = new Artwork
        {
            Id = Guid.NewGuid(),
            Title = artwork.Title,
            AccountId = artwork.AccountId,
            Description = artwork.Description,
            Url = artwork.Url,
            Likes = artwork.Likes,
            Fee = artwork.Fee,
            Status = artwork.Status
        };
        _context.Artworks.Add(createArtwork);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
    }

    public async Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate artwork)
    {
        var imageExist = await _context.Artworks.FirstOrDefaultAsync(c =>
            c.AccountId.Equals(artwork.AccountId) && c.Title.ToLower().Equals(artwork.Title.ToLower()));
        if (imageExist == null) return new StatusCodeResult(409);
        imageExist = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(artwork.Id));
        if (!string.IsNullOrEmpty(artwork.Title)) imageExist.Title = artwork.Title;
        if (!string.IsNullOrEmpty(artwork.Url)) imageExist.Url = artwork.Url;
        if (!string.IsNullOrEmpty(artwork.Description)) imageExist.Description = artwork.Description;
        if (artwork.Fee != null) imageExist.Fee = artwork.Fee;
        if (artwork.Status != null) imageExist.Status = artwork.Status;
        if (artwork.Likes != null) artwork.Likes = artwork.Likes;
        if (artwork.AccountId != null) imageExist.AccountId = artwork.AccountId;

        _context.Artworks.Update(imageExist);

        _context.Entry(imageExist).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return new StatusCodeResult(200);
    }

    public async Task<IActionResult> DeleteArtworkAsync(Guid id)
    {
        var artwork = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(id));
        if (artwork == null || artwork.Status.Equals("Disabled")) return new StatusCodeResult(404);
        artwork.Status = "Disabled";
        _context.Artworks.Update(artwork);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(200);
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

    public async Task<List<Artwork>> GetArtworkByArtistId(Guid artistId)
    {
        return await _context.Set<Artwork>().Where(c => c.AccountId.Equals(artistId)).ToListAsync();
    }
}