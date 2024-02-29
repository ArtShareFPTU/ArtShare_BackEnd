using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class ProfilePage : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _accountManage = "https://localhost:7168/api/";

    public ProfilePage(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public AccountResponse Accounts { get; set; }
    public List<ArtworkRespone> Artwork { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient();
        Guid id = Guid.Parse("4e0bd8d4-aa01-4b6b-86a0-e214af826f2a");
        var account = await GetAccountById(id,client);
        var artwork = await GetArtworkByArtistId(id,client);
        if (account == null || artwork is null)
        {
            return NotFound();
        }
        else
        {
            Accounts = account;
            Artwork = artwork;
        }
        return Page();
    }
    private async Task<AccountResponse> GetAccountById(Guid id, HttpClient client)
    {
        var endpoint = _accountManage + $"Account/GetAccount/{id}";
        var response = await client.GetAsync(endpoint);
        if(response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountResponse>(content);

            return result;
        }
        return null;
    }
    private async Task<List<ArtworkRespone>> GetArtworkByArtistId(Guid artistId, HttpClient client)
    {
        var endpoint = _accountManage + $"Artwork/GetArtworksByArtistId/{artistId}";
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ArtworkRespone>>(content);

            return result;
        }
        return null;
    }
}