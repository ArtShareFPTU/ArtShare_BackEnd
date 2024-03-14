using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response;
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

    [BindProperty] public List<Artwork> Artwork { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? search)
    {
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("Token");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artworks = await GetArtworks(client);
        if (artworks == null)
        {
            return NotFound();
        }
        else
        {
            var checkRole = HttpContext.Session.GetString("Role");
            if(checkRole == "Admin")
            {
                return RedirectToPage("/Admin/Index");
            }
            if (search == null || search.Length == 0)
                Artwork = artworks;
            else
            {
                var endpoint = _artworkManage + $"GetArtworkFromSearch/resource?search={search}";
                var response = await client.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<Artwork>>(content);

                    Artwork = result;
                }
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

    public IActionResult OnGetLogout()
    {
        HttpContext.Session.Remove("Token");
        return RedirectToPage();
    }
}