using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOS;
using ModelLayer.DTOS.Response;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class DetailPageModel : PageModel
{
    private readonly HttpClient _client = new HttpClient();
    
    private readonly IConfiguration _configuration;
        
    public DetailPageModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public ArtworkRespone ArtworkRespone { get; set; } = default!; 

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
    

    
}