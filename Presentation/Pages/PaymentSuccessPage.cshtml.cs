using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Order;
using ModelLayer.Enum;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class PaymentSuccessPage : PageModel
{
    private readonly HttpClient _client = new HttpClient();

    [BindProperty] public Order Order { get; set; }
    public string SaveToken { get; set; }

    public async void OnGet(string token, string PayerID)
    {
        var result = new UpdateToken()
        {
            token = token,
            result = OrderStatus.Completed.ToString()
        };
        var json = JsonConvert.SerializeObject(result);

        SaveToken = token;
        
        var key = HttpContext.Session.GetString("Token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var respone = await _client.PutAsync("https://localhost:7168/api/Order/UpdateStatusOrder", content);
        if (respone.IsSuccessStatusCode)
        {
        }
    }

    public async Task<IActionResult> OnPost(string token)
    {
        var key = HttpContext.Session.GetString("Token");
        if (key == null || key.Length == 0) return RedirectToPage("./Logout");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        
        
        // Lấy Order có chứa artwork
        var order = await _client.GetAsync($"https://localhost:7168/api/Order/GetOrderByToken/{token}");
        if (order.IsSuccessStatusCode)
        {
            var jsonString = await order.Content.ReadAsStringAsync();
            Order = JsonConvert.DeserializeObject<Order>(jsonString);
        }

        //dùng vong lặp trong trường hợp mua nhiều ảnh
        // nhung chỉ tai dc 1 ảnh do return
        foreach (var item in Order.OrderDetails)
        {
            string url = "https://localhost:7168/api/Artwork/DownloadImage?id=" + item.ArtworkId;
            var downloadImage = await _client.GetAsync(url);
            if (downloadImage.IsSuccessStatusCode)
            {
                var fileName = downloadImage.Content.Headers.ContentDisposition.FileName;
                fileName = RemovePaidVersionFromFileName(fileName);
                var fileData = await downloadImage.Content.ReadAsByteArrayAsync(); 
                return File(fileData, "image/jpg", fileName);
            }
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