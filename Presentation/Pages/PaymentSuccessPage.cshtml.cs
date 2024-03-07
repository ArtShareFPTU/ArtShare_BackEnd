using System.Net.Http.Headers;
using System.Text;
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
}