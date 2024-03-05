using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Presentation.Pages;

public class ProfilePage : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _accountManage = "https://localhost:7168/api/";
    private readonly IConfiguration _configuration;

    public ProfilePage(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    public AccountResponse Accounts { get; set; }
    public List<ArtworkRespone> Artwork { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("Token");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        Guid id = Guid.Parse(GetIdFromJwt(key));
        var account = await GetAccountById(id,client);
        var artwork = await GetArtworkByArtistId(id,client);
        if (account == null)
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
    public async Task<IActionResult> OnGetArtistProfile(Guid id)
    {
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("Token");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var account = await GetAccountById(id, client);
        var artwork = await GetArtworkByArtistId(id, client);
        if (account == null)
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
    public string GetIdFromJwt(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Tokens:Key"]);

        tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        }, out SecurityToken validatedToken);

        var jwtTokenDecoded = (JwtSecurityToken)validatedToken;

        // Truy cập vào các thông tin trong payload
        string userId = jwtTokenDecoded.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

        return userId;
    }
}