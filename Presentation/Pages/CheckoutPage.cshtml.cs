using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BusinessLogicLayer.PayPal;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOS;
using Newtonsoft.Json;
using PayPal.Models;
using PayPal.Models.Requests;
using Amount = PayPal.Models.Requests.Amount;

//using PayPal.Api;


namespace Presentation.Pages;

public class CheckoutPage : PageModel
{
    private IHttpContextAccessor httpContextAccessor;
    private readonly HttpClient _client = new HttpClient();
    private readonly IConfiguration _configuration;
    private readonly PayPalClientApi PayPalClientApi = new PayPalClientApi();

    public CheckoutPage(IHttpContextAccessor context, IConfiguration configuration)
    {
        httpContextAccessor = context;
        _configuration = configuration;
    }

    public List<Carts> CartsList { get; set; }

    public async Task OnGetAsync()
    {
        // get cart from session
        var json = HttpContext.Session.GetString("cart");
        // deserialize cart
        if (json != null) CartsList = JsonConvert.DeserializeObject<List<Carts>>(json);
    }
    
    
    public async Task OnPostAsync()
    {
        // get cart from session
        var json = HttpContext.Session.GetString("cart");
        // deserialize cart
        if (json != null) CartsList = JsonConvert.DeserializeObject<List<Carts>>(json);
        
        var token = await PayPalClientApi.GetAuthorizationRequest();
        

        var list = new List<Item>();
        foreach (var c in CartsList)
        {
            var item = new Item();
            item.name = c.Title;
            item.quantity = "1";
            item.currency = "USD";
            item.price = c.Price.ToString();
            list.Add(item);
        }
        
        var PurchaseUnit = new PurchaseUnit()
        {
            reference_id = Guid.NewGuid().ToString(),
            description = "Buy ArtWork Online",
            items = list,
            amount = new Amount()
            {
                currency = "USD",
                total = CartsList.Sum(i => i.Price).ToString()
            }
        };
        
        var requestContent = JsonConvert.SerializeObject(PurchaseUnit);
        _client.SetBearerToken(token.access_token);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _client.PostAsync("https://api-m.paypal.com/v1/checkout/orders", content).Result;
        
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