using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOS;
using Newtonsoft.Json;
/*using PayPal.Api;*/


namespace Presentation.Pages;

public class CheckoutPage : PageModel
{
    private IHttpContextAccessor httpContextAccessor;
    private readonly HttpClient _client = new HttpClient();
    private readonly IConfiguration _configuration;

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
    
}