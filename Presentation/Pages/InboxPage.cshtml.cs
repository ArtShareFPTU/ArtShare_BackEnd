using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Inbox;
using ModelLayer.DTOS.Response.Inbox;
using Newtonsoft.Json;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Presentation.Pages
{
    public class InboxPageModel : PageModel
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public string Username { get; set; }
        [BindProperty] public InboxCreation inbox { get; set; }
        [BindProperty] public InboxDetailResponse InboxDetailResponse { get; set; } = default!;
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
            
            Username = GetUserNameFromJwt(accessToken);
            var id = GetIdFromJwt(accessToken);
            var responseReceiver =
                await _client.GetAsync($"https://localhost:7168/api/Inbox/GetInboxReceiverResponses/{id}");
            if (responseReceiver.IsSuccessStatusCode)
            {
                var contentReceiver = await responseReceiver.Content.ReadAsStringAsync();
                InboxReceiverResponses =  JsonConvert.DeserializeObject<List<InboxReceiverResponse>>(contentReceiver);
            }

            var responseSender =
                await _client.GetAsync($"https://localhost:7168/api/Inbox/GetInboxSenderResponses/{id}");
            if (responseSender.IsSuccessStatusCode)
            {
                var contentReceiver = await responseSender.Content.ReadAsStringAsync();
                InboxSenderResponses = JsonConvert.DeserializeObject<List<InboxSenderResponse>>(contentReceiver);
            }
        }

        public async Task<IActionResult> OnPostSend()
        {
            try
            {
                var accessToken = HttpContext.Session.GetString("Token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                var responseID = await _client.GetAsync(
                    $"https://localhost:7168/api/Account/GetAccountByUserName/{Request.Form["inbox.ReceiverId"]}");
                if (!responseID.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Can Not find User Name.");
                    return Page();
                }

                var receiverId = await responseID.Content.ReadAsStringAsync();


                inbox.ReceiverId = Guid.Parse(JsonConvert.DeserializeObject(receiverId).ToString());
                inbox.SenderId = Guid.Parse(GetIdFromJwt(accessToken));

                if (inbox.ReceiverId == inbox.SenderId)
                {
                    ModelState.AddModelError(string.Empty, "You can not sand to you.");
                    return Page();
                }

                // Prepare multipart form data
                using var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(inbox.SenderId.ToString()), "SenderId");
                formData.Add(new StringContent(inbox.ReceiverId.ToString()), "ReceiverId");
                formData.Add(new StringContent(inbox.Title), "Title");
                formData.Add(new StringContent(inbox.Content), "Content");

                if (Request.Form.Files.Count > 0)
                {
                    formData.Add(new StreamContent(Request.Form.Files[0].OpenReadStream()), "file",
                        Request.Form.Files[0].FileName);
                }
                else
                {
                    formData.Add(new StringContent(string.Empty), "Image");
                }
                
                // Send the message
                var response = await _client.PostAsync("https://localhost:7168/api/Inbox/CreateInbox", formData);

                // Check response status and redirect accordingly
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("InboxPage");
                }
                else
                {
                    // Handle error
                    // You may want to return a specific view or provide an error message
                    return BadRequest("Error sending message");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                // You may want to log the exception for debugging purposes
                return BadRequest("An error occurred: " + ex.Message);
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

        public string GetUserNameFromJwt(string jwtToken)
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
            string user = jwtTokenDecoded.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;

            return user;
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
            OnGet();
            return Page();
        }
        
        public async Task<IActionResult> OnGetReceiver(Guid id)
        {
            var accessToken = HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _client.GetAsync($"https://localhost:7168/api/Inbox/GetInboxDetail/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                InboxDetailResponse = JsonConvert.DeserializeObject<InboxDetailResponse>(content);
            }
            await OnGet();
            return Page();
        }
        
    }
}