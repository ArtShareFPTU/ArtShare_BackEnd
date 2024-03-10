using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS;
using ModelLayer.DTOS.Request.Order;
using ModelLayer.PayPal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AuthenticationHeaderValue = System.Net.Http.Headers.AuthenticationHeaderValue;


namespace Presentation.Pages;

public class CheckoutPage : PageModel
{
    private readonly PayPalConfig _payPalConfig;
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public CheckoutPage(IOptions<PayPalConfig> paypalConfig, IConfiguration configuration)
    {
        _payPalConfig = paypalConfig.Value;
        _client = new HttpClient();
        _configuration = configuration;
    }

    public List<Carts> CartsList { get; set; }
    public decimal Amount { get; set; }

    public async Task OnGetAsync()
    {
        // get cart from session
        var json = HttpContext.Session.GetString("cart");
        // deserialize cart
        if (json != null) CartsList = JsonConvert.DeserializeObject<List<Carts>>(json);

        Amount = CartsList.Sum(c => c.Price);
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var accessToken = HttpContext.Session.GetString("Token");
        if (accessToken == null)
        {
            return RedirectToPage("LoginPage");
        }
        
        // get cart from session
        var jsonCart = HttpContext.Session.GetString("cart");

        //set request
        if (jsonCart != null) CartsList = JsonConvert.DeserializeObject<List<Carts>>(jsonCart);

        var token = await GetToken();
        var json = await GetJson(CartsList);

        var orderId = await CreateOrder(CartsList);

        _client.DefaultRequestHeaders.Add("PayPal-Partner-Attribution-Id", Guid.NewGuid().ToString());
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var httpRequestMessage = new HttpRequestMessage
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //handle response
        var response = await _client.PostAsync($"{_payPalConfig.BaseUrl}/v1/checkout/orders", httpRequestMessage.Content);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        //Remove session
        HttpContext.Session.Remove("cart");
        
        //get link approval 
        string link = await GetApprovalUrl(responseContent);
        
        //get id token and update to DB
        JObject jsonObject = JObject.Parse(responseContent);
        string id = (string)jsonObject["id"];
        await CreateTokenInOrder(id, orderId);
        return Redirect($"{link}");
    }

    public async Task<Guid> CreateOrder(List<Carts> cartsList)
    {
        var accessToken = HttpContext.Session.GetString("Token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var cartsJson = JsonConvert.SerializeObject(cartsList);
        var content = new StringContent(cartsJson, Encoding.UTF8, "application/json");

        var id = GetIdFromJwt(accessToken);

        var response = await _client.PostAsync($"https://localhost:7168/api/Order/PostOrder?customerId={id}",
            content);
        if (response.IsSuccessStatusCode)
        {
            var order = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Order>(order);
            return result.Id;
        }
        return Guid.Empty;
    }
    
    
    public async Task CreateTokenInOrder(string token, Guid orderId)
    {
        var accessToken = HttpContext.Session.GetString("Token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var createToken = new CreateToken()
        {
            Id = orderId,
            Token = token
        };
        
        var json = JsonConvert.SerializeObject(createToken);
        
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"https://localhost:7168/api/Order/CreateTokenOrder", content);
    }


    public async Task<String> GetJson(List<Carts> cartsList)
    {
        // get cart from session
        var jsonCart = HttpContext.Session.GetString("cart");

        if (jsonCart != null) CartsList = JsonConvert.DeserializeObject<List<Carts>>(jsonCart);


        //Create the Body 
        redirect_urls urls = new redirect_urls()
        {
            return_url = "https://localhost:7120/PaymentSuccessPage",
            cancel_url = "https://localhost:7120/PaymentFailedPage"
        };

        var items = new List<items>();
        foreach (var c in CartsList)
        {
            var item = new items();
            item.name = c.Title;
            item.quantity = "1";
            item.price = c.Price.ToString();
            item.currency = "USD";
            items.Add(item);
        }

        var purchase_units = new List<purchase_units>();

        var purchase_unit = new purchase_units()
        {
            reference_id = Guid.NewGuid().ToString(),
            amount = new amount()
            {
                currency = "USD",
                total = CartsList.Sum(x => x.Price).ToString()
            },
            items = items
        };

        purchase_units.Add(purchase_unit);

        var jsonUrl = JsonConvert.SerializeObject(urls);
        var jsonpurchase_units = JsonConvert.SerializeObject(purchase_units);

        JObject o1 = JObject.Parse(jsonUrl);
        JArray o2 = JArray.Parse(jsonpurchase_units);

        JObject combinedObject = new JObject();
        combinedObject["purchase_units"] = o2;
        combinedObject["redirect_urls"] = o1;

        return combinedObject.ToString();
    }


    public async Task<string> GetToken()
    {
        var byteArray = Encoding.ASCII.GetBytes($"{_payPalConfig.ClientId}:{_payPalConfig.ClientSecret}");
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var keyValueParis = new List<KeyValuePair<string, string>>
            { new KeyValuePair<string, string>("grant_type", "client_credentials") };

        var response = await _client.PostAsync($"{_payPalConfig.BaseUrl}/v1/oauth2/token",
            new FormUrlEncodedContent(keyValueParis));

        var responseAsString = await response.Content.ReadAsStringAsync();

        var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponseData>(responseAsString);

        return authorizationResponse.access_token;
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

    public async Task<string> GetApprovalUrl(string responseContent)
    {
        // Phân tích phản hồi JSON
        JObject responseObject = JObject.Parse(responseContent);

        // Lấy danh sách các liên kết từ phản hồi JSON
        JArray linksArray = (JArray)responseObject["links"];

        // Tìm liên kết có thuộc tính "rel" bằng "approval_url"
        string approvalUrl = linksArray
            .FirstOrDefault(link => link["rel"].ToString() == "approval_url")
            ?.Value<string>("href");

        return approvalUrl;
    }
}