using System.Runtime.CompilerServices;
using System.Text.Json;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Inbox;

namespace DataAccessLayer.BussinessObject.Repository;

public class InboxRepository : IInboxRepository
{
    private readonly ArtShareContext _context;

    public InboxRepository()
    {
        _context = new ArtShareContext();
    }
    
    public async Task<List<Inbox>> GetInboxByReceiverIdAsync(Guid id)
    {
        return await _context.Inboxes.Include(i => i.Sender).Where(i => i.ReceiverId == id).ToListAsync();
    }

    public async Task<List<Inbox>> GetInboxBySenderIdAsync(Guid id)
    {
        return await _context.Inboxes.Include(i => i.Receiver).Where(i => i.SenderId == id).ToListAsync();
    }

    public async Task<Inbox> GetInboxByIdAsync(Guid id)
    {
        return await _context.Inboxes.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IActionResult> CreateInboxAsync(InboxCreation item)
    {
        var fileExtension = Path.GetExtension(item.file.FileName)?.ToLower();
        if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png"
            && fileExtension != ".bmp" && fileExtension != ".gif" && fileExtension != ".tiff"
            && fileExtension != ".webp" && fileExtension != ".heic" && fileExtension != ".pdf")
            return new StatusCodeResult(415);
        byte[] fileBytes;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await item.file.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        var imgbbPremiumURL = await UploadToImgBB(fileBytes, item.Title + "(Request)");
        if (imgbbPremiumURL == null) return new StatusCodeResult(500);
        string fileData = imgbbPremiumURL.ToString();
        var inbox = new Inbox
        {
            Id = Guid.NewGuid(),
            SenderId = item.SenderId,
            ReceiverId = item.ReceiverId,
            Title = item.Title,
            file = fileData,
            Content = item.Content,
            CreateDate = DateTime.Now
        };
        _context.Inboxes.Add(inbox);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(201);
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
}