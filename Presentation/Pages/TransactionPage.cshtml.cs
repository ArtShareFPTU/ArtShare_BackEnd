using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Presentation.Pages
{
    public class TransactionPageModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _orderManage = "https://localhost:7168/api/";
        private readonly IConfiguration _configuration;

        public TransactionPageModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public AccountResponse Accounts { get; set; }
        [BindProperty] public List<Order> orders { get; set; }
        [BindProperty] public List<OrderDetail> orderdetail { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            Guid id = Guid.Parse(GetIdFromJwt(key));
            var ord = await GetOrderByAccountId(id, client);
            var ordetails = await GetOrderDetailByAccountId(id, client);
            if (ord == null)
            {
                return NotFound();
            }
            else
            {
                orders = ord;
                orderdetail = ordetails;
            }
            return Page();
        }
        private async Task<List<Order>> GetOrderByAccountId(Guid id, HttpClient client)
        {
            var endpoint = _orderManage + $"Order/GetOrderByAccountId/{id}";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Order>>(content);

                return result;
            }
            return null;
        }
        private async Task<List<OrderDetail>> GetOrderDetailByAccountId(Guid id, HttpClient client)
        {
            var endpoint = _orderManage + $"Order/GetOrderDetailByAccountId/{id}";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<OrderDetail>>(content);

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
}
