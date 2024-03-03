using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOS;
using ModelLayer.DTOS.Response;
using Newtonsoft.Json;

namespace Presentation.Pages;

public class CheckoutPage : PageModel
{
    private readonly HttpClient _client = new HttpClient();
    private readonly IConfiguration _configuration;
        
    public CheckoutPage(IConfiguration configuration)
    {
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