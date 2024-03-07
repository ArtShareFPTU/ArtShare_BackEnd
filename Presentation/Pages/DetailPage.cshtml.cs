using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Request.Comment;
using ModelLayer.DTOS.Response;
using ModelLayer.DTOS.Response.Account;
using ModelLayer.DTOS.Response.Commons;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class DetailPageModel : PageModel
{
    private readonly HttpClient _client = new HttpClient();
    
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountResponse Accounts { get; set; }

    public DetailPageModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }
    
    [BindProperty]
    public CommentCreation commentCreation { get; set; }
    public ArtworkRespone ArtworkRespone { get; set; } = default!;
    private readonly string _accountManage = "https://localhost:7168/api/";

    public async Task OnGetAsync(Guid id)
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
    
    public async Task<IActionResult> OnPostAddToCart(Guid id)
    {
        var check = false;
        List<Carts> cartsList = new();
        Carts newCart = new();
        var artwork = await _client.GetAsync($"https://localhost:7168/api/Artwork/GetArtworkById/{id}");
        if (artwork.IsSuccessStatusCode)
        {
            var jsonString = await artwork.Content.ReadAsStringAsync();

            ArtworkRespone = JsonConvert.DeserializeObject<ArtworkRespone>(jsonString);
        }
        // get cart from session
        var json = HttpContext.Session.GetString("cart");
        // deserialize cart
        if (json != null) cartsList = JsonConvert.DeserializeObject<List<Carts>>(json);
        // add book to cart
        if (!cartsList.Any(c => c.Id == id))
        {
            newCart.Id = ArtworkRespone.Id;
            newCart.Title = ArtworkRespone.Title;
            newCart.Price = ArtworkRespone.Fee.Value;
            newCart.ImageUrl = ArtworkRespone.Url;
            cartsList.Add(newCart);
        }
        //Remove old Session
        HttpContext.Session.Remove("cart");
        // serialize cart
        json = JsonConvert.SerializeObject(cartsList);
        HttpContext.Session.SetString("cart", json);
        return  RedirectToPage();
    }


    
    public async Task<IActionResult> OnPostCreateComment()
    {
        
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            // Th?c hi?n m?t s? logic ?? l?y accountId t? token JWT
            var accountId = GetIdFromJwt(key);

            // Ki?m tra accountId có giá tr? không
            if (accountId == null)
            {
                // Thông báo l?i n?u không tìm th?y AccountId t? token
                ModelState.AddModelError("", "AccountId not found in JWT token.");
                return Page();
            }

            // Gán AccountId vào commentCreation
            commentCreation.AccountId = Guid.Parse(accountId);

            // Gán ArtworkId vào commentCreation
            //commentCreation.ArtworkId = id;
            commentCreation.ArtworkId = Guid.Parse(Request.Form["ArtworkRespone.Id"]);
            var response = await client.PostAsJsonAsync("https://localhost:7168/api/Comment/PostComment/create", commentCreation);
            
                
                return RedirectToPage(new { commentCreation.ArtworkId });
            
            
        
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

        // Truy c?p vào các thông tin trong payload
        string userId = jwtTokenDecoded.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

        return userId;
    }

}