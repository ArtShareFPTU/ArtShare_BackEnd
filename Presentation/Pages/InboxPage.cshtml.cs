using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOS.Response.Inbox;
using Newtonsoft.Json;

namespace Presentation.Pages
{
    public class InboxPageModel : PageModel
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        
        
        [BindProperty]public InboxDetailResponse InboxDetailResponse { get; set; }  = default!;
        [BindProperty] public List<InboxReceiverResponse> InboxReceiverResponses { get; set; } = default!;
        [BindProperty] public List<InboxSenderResponse> InboxSenderResponses { get; set; } = default!;

        public InboxPageModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
        }
        
        public async Task OnGet()
        { 
            var accessToken = HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var id = GetIdFromJwt(accessToken);
            var responseReceiver = await _client.GetAsync($"https://localhost:7168/api/Inbox/GetInboxReceiverResponses/{id}");
            if (responseReceiver.IsSuccessStatusCode)
            {
                var contentReceiver = await responseReceiver.Content.ReadAsStringAsync();
                InboxReceiverResponses = JsonConvert.DeserializeObject<List<InboxReceiverResponse>>(contentReceiver);              
            }
            var responseSender = await _client.GetAsync($"https://localhost:7168/api/Inbox/GetInboxSennderResponses/{id}");
            if (responseSender.IsSuccessStatusCode)
            {
                var contentReceiver = await responseSender.Content.ReadAsStringAsync();
                InboxSenderResponses = JsonConvert.DeserializeObject<List<InboxSenderResponse>>(contentReceiver);              
            }
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