using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class HomePage : PageModel
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _artworkManage = "https://localhost:7168/api/Artwork/";


    public HomePage(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public List<Artwork> Artwork { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? search)
    {
        var client = _httpClientFactory.CreateClient();
        
        //var key = HttpContext.Session.GetString("key");
        //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artworks = await GetArtworks(client);
        if (artworks == null)
        {
            return NotFound();
        }
        else
        {
            if(search == null || search.Length == 0)
            {
               
                Artwork = artworks;
            }
            else
            {
                
                Artwork = artworks.Where(c => c.Title.ToLower().Contains(search.ToLower())).ToList();
            }
            
        }
        return Page();
    }

    private async Task<List<Artwork>> GetArtworks(HttpClient client)
    {
        var endpoint = _artworkManage + "GetArtworks";
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Artwork>>(content);

            return result;
        }
        return null;
    }
}