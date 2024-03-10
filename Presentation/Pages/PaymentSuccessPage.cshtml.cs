using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.DTOS.Request.Order;
using ModelLayer.Enum;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class PaymentSuccessPage : PageModel
{
    private readonly HttpClient _client = new HttpClient();
    public async void OnGet(string token, string PayerID)
    {
        var result = new UpdateToken()
        {
            token = token,
            result = OrderStatus.Completed.ToString()
        };
        var json = JsonConvert.SerializeObject(result);
        
        var key = HttpContext.Session.GetString("Token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var respone = await _client.PutAsync("https://localhost:7168/api/Order/UpdateStatusOrder", content);
        if (!respone.IsSuccessStatusCode)
        {
        }  
    }
    public async Task<IActionResult> OnPost()
    {
        var imageId = HttpContext.Session.GetString("imageId");
        if (!Guid.TryParse(imageId, out Guid id))
        {
            // Handle invalid imageId
            return BadRequest("Invalid imageId");
        }
        string url = "https://localhost:7168/api/Artwork/DownloadImage?id=" + id;
        var downloadImage = await _client.GetAsync(url);
        if (downloadImage.IsSuccessStatusCode)
        {
            var fileName = downloadImage.Content.Headers.ContentDisposition.FileName;
            fileName = RemovePaidVersionFromFileName(fileName);
            var fileData = await downloadImage.Content.ReadAsByteArrayAsync();
            return File(fileData, "image/jpg", fileName);
        }
        return Page();
    }
    private string RemovePaidVersionFromFileName(string fileName)
    {
        const string paidVersion = "-Paid-version";

        int paidVersionIndex = fileName.IndexOf(paidVersion);
        if (paidVersionIndex != -1)
        {
            // Loại bỏ phần "Paid version" và khoảng trắng phía sau nếu có
            fileName = fileName.Remove(paidVersionIndex, paidVersion.Length).TrimEnd();
        }

        return fileName;
    }
}