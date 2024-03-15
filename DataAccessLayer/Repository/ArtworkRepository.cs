using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
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
        return await _context.Artworks.Include(a => a.Account).OrderByDescending(c => c.Likes).ToListAsync();
    }

    public async Task<Artwork> GetArtworkByIdAsync(Guid id)
    {
        return await _context.Artworks.Include(a => a.ArtworkCategories)
            .Include(a => a.Account)
            .Include(a => a.ArtworkTags).ThenInclude(t => t.Tag)
            .Include(a => a.ArtworkCategories).ThenInclude(c => c.Category)
            .Include(a => a.Comments)
            .Include(a => a.LikesNavigation)
            .FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public async Task<IActionResult> AddArtworkAsync(ArtworkCreation artwork)
    {
        var artworkId = Guid.NewGuid();
        string? imgbbURL = string.Empty;
        string? imgbbURL_Premium = string.Empty;
        if (artwork.Image == null) return new StatusCodeResult(500);
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

            imgbbURL_Premium = await UploadToImgBB(imageData, artwork.Title + "(Paid version)");
            if (imgbbURL_Premium == null) return new StatusCodeResult(500);

            using var image = SixLabors.ImageSharp.Image.Load(imageData);
            image.Mutate(x => x.Resize(640, 360)); 

            using (var outputStream = new MemoryStream())
            {
                image.SaveAsJpeg(outputStream);
                imageData = outputStream.ToArray();
            }

            imgbbURL = await UploadToImgBB(imageData, artwork.Title);
            if (imgbbURL == null) return new StatusCodeResult(500);
        }

        var imageExist = await _context.Artworks.FirstOrDefaultAsync(c =>
            c.AccountId.Equals(artwork.AccountId) && c.Title.ToLower().Equals(artwork.Title.ToLower()));
        if (imageExist != null) return new StatusCodeResult(409);

        var categoriesGuid = artwork.ArtworkCategories;
        if (categoriesGuid == null) return new StatusCodeResult(409);
        foreach (var category in categoriesGuid)
        {
            if (await _context.Categories.AnyAsync(c => c.Id.Equals(category)) == null) return new StatusCodeResult(409);
            ArtworkCategory artworkCategory = new ArtworkCategory
            {
                Id = Guid.NewGuid(),
                ArtworkId = artworkId,
                CategoryId = category
            };
            _context.ArtworkCategories.Add(artworkCategory);
        }
        var tagsGuid = artwork.ArtworkTags;
        foreach (var tag in tagsGuid)
        {
            if (!await _context.Tags.AnyAsync(c => c.Id.Equals(tag)) == null) return new StatusCodeResult(409);
            ArtworkTag artworkTag = new ArtworkTag { Id = Guid.NewGuid(), ArtworkId = artworkId, TagId = tag };
        }

        var createArtwork = new Artwork
        {
            Id = artworkId,
            Title = artwork.Title,
            AccountId = artwork.AccountId,
            Description = artwork.Description,
            Likes = artwork.Likes,
            Url = imgbbURL,
            PremiumUrl = imgbbURL_Premium,
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
            c.Id.Equals(artwork.Id));
        if (imageExist == null) return new StatusCodeResult(409);
        imageExist = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(artwork.Id));
        if (!string.IsNullOrEmpty(artwork.Title)) imageExist.Title = artwork.Title;
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
            var imgbbPremiumURL = await UploadToImgBB(imageData, artwork.Title + "(Paid version)");
            if (imgbbPremiumURL == null) return new StatusCodeResult(500);

            // Resize the image using ImageSharp
            using var image = SixLabors.ImageSharp.Image.Load(imageData);
            image.Mutate(x => x.Resize(640, 360));

            // Convert the image back to a byte array
            using (var outputStream = new MemoryStream())
            {
                image.SaveAsJpeg(outputStream);
                imageData = outputStream.ToArray();
            }

            // Save resized image to ImgBB
            var imgbbURL = await UploadToImgBB(imageData, artwork.Title);
            if (imgbbURL == null) return new StatusCodeResult(500);

            imageExist.Url = imgbbURL;
            imageExist.PremiumUrl = imgbbPremiumURL;
        }
        if (!string.IsNullOrEmpty(artwork.Description)) imageExist.Description = artwork.Description;
        if (artwork.Fee != null) imageExist.Fee = artwork.Fee;
        if (artwork.Status != null) imageExist.Status = artwork.Status;
        if (artwork.Likes != null) artwork.Likes = artwork.Likes;
        if (artwork.AccountId != null) imageExist.AccountId = artwork.AccountId;

        var categoriesArtworkCurrent = await _context.ArtworkCategories.Where(c => c.ArtworkId.Equals(artwork.Id)).ToListAsync();
        if (categoriesArtworkCurrent == null) return new StatusCodeResult(409);

        var tagArtworkCurrent = await _context.ArtworkTags.Where(c => c.ArtworkId.Equals(artwork.Id)).ToListAsync();
        if (tagArtworkCurrent == null) return new StatusCodeResult(409);

        _context.ArtworkCategories.RemoveRange(categoriesArtworkCurrent);
        _context.ArtworkTags.RemoveRange(tagArtworkCurrent);

        var categoriesGuid = artwork.ArtworkCategories;
        if (categoriesGuid == null) return new StatusCodeResult(409);
        foreach (var category in categoriesGuid)
        {
            if (await _context.Categories.AnyAsync(c => c.Id.Equals(category)) == null) return new StatusCodeResult(409);
            var artworkCategories = await _context.ArtworkCategories.Where(c => c.ArtworkId.Equals(artwork.Id)).ToListAsync();
            ArtworkCategory artworkCategory = new ArtworkCategory
            {
                Id = Guid.NewGuid(),
                ArtworkId = artwork.Id,
                CategoryId = category
            };
            _context.ArtworkCategories.Add(artworkCategory);
        }
        var tagsGuid = artwork.ArtworkTags;
        foreach (var tag in tagsGuid)
        {
            if (!await _context.Tags.AnyAsync(c => c.Id.Equals(tag)) == null) return new StatusCodeResult(409);
            ArtworkTag artworkTag = new ArtworkTag { Id = Guid.NewGuid(), ArtworkId = artwork.Id, TagId = tag };
        }

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
    public async Task<IActionResult> AddCategory4Artwork(ArtworkCategoryAddition artworkCategoryAddition)
    {
        var artwork = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(artworkCategoryAddition.ArtworkId));
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(artworkCategoryAddition.CategoryId));

        if (category == null || artwork == null) return new StatusCodeResult(409);
        ArtworkCategory artworkCategory = new ArtworkCategory
        {
            ArtworkId = artworkCategoryAddition.ArtworkId,
            CategoryId = artworkCategoryAddition.CategoryId,
            Id = Guid.NewGuid()
        };
        _context.ArtworkCategories.Add(artworkCategory);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
    }
    public async Task<IActionResult> UpdateCategory4Artwork(ArtworkCategoryUpdate artworkCategoryUpdate)
    {
        var artwork = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(artworkCategoryUpdate.ArtworkId));
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(artworkCategoryUpdate.CategoryId));
        ArtworkCategory artworkCategory = await _context.ArtworkCategories.FirstOrDefaultAsync(c => c.Id.Equals(artworkCategoryUpdate.Id));
        if (category == null || artwork == null || artworkCategory == null || artwork.Id.Equals(artworkCategoryUpdate.ArtworkId) && category.Id.Equals(artworkCategoryUpdate.CategoryId)) return new StatusCodeResult(409);
        artworkCategory.ArtworkId = artworkCategoryUpdate.ArtworkId;
        artworkCategory.CategoryId = artworkCategoryUpdate.CategoryId;
        _context.ArtworkCategories.Update(artworkCategory);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(200);
    }
    public async Task<IActionResult> AddTag4Artwork(ArtworkTagAddition artworkTagAddition)
    {
        var artwork = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(artworkTagAddition.ArtworkId));
        var category = await _context.Tags.FirstOrDefaultAsync(c => c.Id.Equals(artworkTagAddition.TagId));

        if (category == null || artwork == null) return new StatusCodeResult(409);
        ArtworkTag artworkTag = new ArtworkTag
        {
            ArtworkId = artworkTagAddition.ArtworkId,
            TagId = artworkTagAddition.TagId,
            Id = Guid.NewGuid()
        };
        _context.ArtworkTags.Add(artworkTag);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
    }
    public async Task<IActionResult> UpdateTag4Artwork(ArtworkTagUpdate artworkTagUpdate)
    {
        var artwork = await _context.Artworks.FirstOrDefaultAsync(c => c.Id.Equals(artworkTagUpdate.ArtworkId));
        var category = await _context.Tags.FirstOrDefaultAsync(c => c.Id.Equals(artworkTagUpdate.TagId));
        ArtworkTag artworkTag = await _context.ArtworkTags.FirstOrDefaultAsync(c => c.Id.Equals(artworkTagUpdate.Id));
        if (category == null || artwork == null || artworkTag == null || artwork.Id.Equals(artworkTagUpdate.ArtworkId) && category.Id.Equals(artworkTagUpdate.TagId)) return new StatusCodeResult(409);
        artworkTag.ArtworkId = artworkTagUpdate.ArtworkId;
        artworkTag.TagId = artworkTagUpdate.TagId;
        _context.ArtworkTags.Update(artworkTag);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(200);
    }
    public async Task<IActionResult> RemoveTag4Artwork(Guid id)
    {
        var artworkTag = await _context.ArtworkTags.FirstOrDefaultAsync(c => c.Id.Equals(id));
        if (artworkTag == null) return new StatusCodeResult(404);
        _context.ArtworkTags.Remove(artworkTag);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(204);
    }
    public async Task<IActionResult> RemoveCategory4Artwork(Guid id)
    {
        var artworkCategory = await _context.ArtworkCategories.FirstOrDefaultAsync(c => c.Id.Equals(id));
        if (artworkCategory == null) return new StatusCodeResult(404);
        _context.ArtworkCategories.Remove(artworkCategory);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(204);
    }
    public async Task<List<ArtworkCategory>> GetArtworkCategoryByArtworkId(Guid id) => await _context.ArtworkCategories.Where(c => c.ArtworkId.Equals(id)).ToListAsync();
    public async Task<List<ArtworkCategory>> GetArtworkCategoryByCategoryId(Guid id) => await _context.ArtworkCategories.Where(c => c.CategoryId.Equals(id)).ToListAsync();
    public async Task<List<ArtworkCategory>> GetArtworkCategories() => await _context.ArtworkCategories.ToListAsync();

    public async Task<List<ArtworkTag>> GetArtworkTagByArtworkId(Guid id) => await _context.ArtworkTags.Where(c => c.ArtworkId.Equals(id)).ToListAsync();
    public async Task<List<ArtworkTag>> GetArtworkTagByTagId(Guid id) => await _context.ArtworkTags.Where(c => c.TagId.Equals(id)).ToListAsync();
    public async Task<List<ArtworkTag>> GetArtworkTags() => await _context.ArtworkTags.ToListAsync();

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

    public async Task<List<Artwork>> GetArtworkFromSearch(string search)
    {
        var checklist = await _context.Set<Artwork>().Include(c => c.ArtworkTags).ThenInclude(c => c.Tag).Include(c => c.ArtworkCategories).ThenInclude(c => c.Category)
        .Where(c => c.Title.Contains(search) || c.ArtworkCategories.Any(at => at.Category.Title.Contains(search)) || c.ArtworkTags.Any(at => at.Tag.Title.Contains(search)))
        .ToListAsync();

        return checklist;
    }

    
}