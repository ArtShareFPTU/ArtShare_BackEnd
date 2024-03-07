using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class ArtworkDetailsModel : PageModel
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _artworkManage = "https://localhost:7168/api/Artwork/";
    private readonly string _tagManage = "https://localhost:7168/api/Tag/";
    private readonly string _categoryManage = "https://localhost:7168/api/Category/";
    private readonly string _likeManage = "https://localhost:7168/api/Like/";
    private readonly string _accountManage = "https://localhost:7168/api/Account/";

    public ArtworkDetailsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Artwork Artwork { get; set; } = default!;
    public Account Account { get; set; } = default!;

    public List<Tag> Tags { get; set; }
    public List<Category> Categories { get; set; }
    public List<Like> Likes { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if (id == null) return NotFound();
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("Token");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artwork = await GetArtwork(client, id);

        if (artwork == null)
        {
            return NotFound();
        }
        else
        {
            Artwork = artwork;
            Tags = await GetTag(client, artwork.Id);
            Categories = await GetCategory(client, artwork.Id);
            Likes = await GetLike(client, artwork.Id);
            Account = await GetAccount(client, artwork.Id);
        }

        return Page();
    }

    private async Task<Artwork> GetArtwork(HttpClient client, Guid id)
    {
        var endpoint = _artworkManage + "GetArtworkById/" + id;
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Artwork>(content);

            return result;
        }

        return null;
    }

    public async Task<List<Tag>> GetTag(HttpClient client, Guid id)
    {
        var endpoint = _tagManage + "GetTagByArtworkId/" + id;
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var tag = JsonConvert.DeserializeObject<List<Tag>>(content);

            return tag;
        }

        return null;
    }

    public async Task<List<Category>> GetCategory(HttpClient client, Guid id)
    {
        var endpoint = _categoryManage + "GetCategoryByArtworkId/" + id;
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Category>>(content);

            return categories;
        }

        return null;
    }

    public async Task<List<Like>> GetLike(HttpClient client, Guid id)
    {
        var endpoint = _likeManage + "GetLikeByArtworkId/" + id;
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Like>>(content);

            return categories;
        }

        return null;
    }

    public async Task<Account> GetAccount(HttpClient client, Guid id)
    {
        var endpoint = _accountManage + "GetAccountByArtworkId/" + id;
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<Account>(content);

            return account;
        }

        return null;
    }
}