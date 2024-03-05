using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;
using System.Net;

namespace Presentation.Pages;

public class DeleteArtworkModel : PageModel
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _artworkManage = "https://localhost:7168/api/Artwork/";
    private readonly string _tagManage = "https://localhost:7168/api/Tag/";
    private readonly string _categoryManage = "https://localhost:7168/api/Category/";
    private readonly string _likeManage = "https://localhost:7168/api/Like/";
    private readonly string _accountManage = "https://localhost:7168/api/Account/";

    public DeleteArtworkModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Artwork Artwork { get; set; } = default!;
    public AccountResponse Account { get; set; } = default!;

    public List<Tag> Tags { get; set; }
    public List<Category> Categories { get; set; }
    public List<Like> Likes { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if (id == null) return NotFound();
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("key");
        if (key == null || key.Length == 0) return RedirectToPage("./LogoutPage");
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
            Account = await GetAccounts(client);
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

    private async Task<AccountResponse> GetAccounts(HttpClient client)
    {
        var endpoint = _accountManage + "GeCurrenttAccount";
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountResponse>(content);

            return result;
        }

        return null;
    }

    private async Task<ActionResult<string>> DeleteArtwork(HttpClient client, Guid id)
    {
        var endpoint = _artworkManage + "RemoveArtwork/remove/" + id;
        var response = await client.PostAsync(endpoint, null);

        var announce = "";
        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.OK) announce = "Artwork has been disabled";
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            announce = "Artwork is not found";
        }
        else if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("./LogoutPage");
        }

        return announce;
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (id == null) return NotFound();
        var client = _httpClientFactory.CreateClient();
        //var key = HttpContext.Session.GetString("key");
        //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var result = await DeleteArtwork(client, id);

        TempData["AnnounceMessage"] = result;
        return Page();
    }
}