using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.DTOS.Response;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class DetailPageModel : PageModel
{
    private readonly HttpClient _client = new HttpClient();
    public ArtworkRespone ArtworkRespone { get; set; } = default!; 

    public async Task OnGetAsync(int id)
    {
        //var accessToken = HttpContext.Session.GetString("account");
        //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var artwork = await _client.GetAsync($"https://localhost:7168/api/Artwork/GetArtworkById/{id}");
        if (artwork.IsSuccessStatusCode)
        {
            var jsonString = await artwork.Content.ReadAsStringAsync();

            ArtworkRespone = JsonConvert.DeserializeObject<ArtworkRespone>(jsonString);
        }
    }
}