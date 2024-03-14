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
    private readonly string _categoryManage = "https://localhost:7168/api/Category/";


    public HomePage(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    [BindProperty]
    public List<Artwork> Artwork { get; set; } = default!;
    public List<Category> Category { get; set; } = default!;
    
    public async Task<IActionResult> OnGetAsync(string? search)
    {
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("Token");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artworks = await GetArtworks(client);
        var categorys = await GetCategorys(client);
        Category = categorys;
        if (artworks == null)
        {
            return NotFound();
        }
        else
        {
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

    public async Task<IActionResult> OnPostCategoryById(Guid categoryId)
    {
        var client = _httpClientFactory.CreateClient();
        var endpoint = _categoryManage + $"GetArtWorkByCategoryId/{categoryId}";
        var response = await client.GetAsync(endpoint);
        if(response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Artwork>>(content);
            Artwork = result;
        }
        var categorys = await GetCategorys(client);
        Category = categorys;
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
    private async Task<List<Category>> GetCategorys(HttpClient client)
    {
        var endpoint = _categoryManage + "GetCategorys";
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Category>>(content);

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