using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Inbox;
using Newtonsoft.Json;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Presentation.Pages
{
    public class InboxPageModel : PageModel
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        
        [BindProperty] public Inbox inbox { get; set; }
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
            var responseSender = await _client.GetAsync($"https://localhost:7168/api/Inbox/GetInboxSenderResponses/{id}");
            if (responseSender.IsSuccessStatusCode)
            {
                var contentReceiver = await responseSender.Content.ReadAsStringAsync();
                InboxSenderResponses = JsonConvert.DeserializeObject<List<InboxSenderResponse>>(contentReceiver);              
            }
        }

        public async Task<IActionResult> OnPostSend()
        {
            var accessToken = HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            //Get Id by Username
            var responseID = await _client.GetAsync($"https://localhost:7168/api/Account/GetAccountByUserName/{inbox.Receiver.UserName}");
            if (!responseID.IsSuccessStatusCode)
            {
                return new EmptyResult();
            }
            var receiverId = await responseID.Content.ReadAsStringAsync();
            
            // Match
            inbox.ReceiverId = Guid.Parse(JsonConvert.DeserializeObject(receiverId).ToString());
            inbox.SenderId = Guid.Parse(GetIdFromJwt(accessToken));
            inbox.Id = Guid.NewGuid();
            var response = _client.PostAsync($"https://localhost:7168/api/Inbox/CreateInbox", new StringContent(JsonConvert.SerializeObject(inbox), Encoding.UTF8, "application/json"));
            
            return RedirectToPage("InboxPage");
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

        public async Task<IActionResult> OnGetSend(Guid id)
        {
            var accessToken = HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            var response = await _client.GetAsync($"https://localhost:7168/api/Inbox/GetInboxDetail/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                InboxDetailResponse = JsonConvert.DeserializeObject<InboxDetailResponse>(content);
            }
            return Page();
        }
        
    }
}
