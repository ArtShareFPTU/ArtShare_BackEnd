using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Response.Account;
using ModelLayer.DTOS.Response.Commons;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Presentation.Pages
{
    public class LoginPageModel : PageModel
    {
		private readonly HttpClient _client = new HttpClient();
		private readonly IConfiguration _configuration;

        public LoginPageModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
		public  LoginAccountResponse accountResponse { get; set; }
		[BindProperty]
		public CreateAccountRequest createAccountRequest { get; set; }

        public async Task<IActionResult> OnPostLogin() {

			var json = JsonSerializer.Serialize(accountResponse);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync("https://localhost:7168/api/Account/Login", content);
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<ServiceResponse<string>>(data);
				if(result.Data != null)
				{
                    var checkRole = GetRoleFromJwt(result.Data);
                    if (checkRole == "User")
                    {
                        HttpContext.Session.SetString("Token", result.Data);
                        HttpContext.Session.SetString("Username", GetUsernameFromJwt(result.Data));
                        HttpContext.Session.SetString("Role", "User");
                        if (GetAvatarFromJwt(result.Data) != null)
                        {
                            HttpContext.Session.SetString("Avatar", GetAvatarFromJwt(result.Data));
                        }
                        return RedirectToPage("/HomePage");
                    }
                    else if(checkRole == "Admin")
                    {
                        HttpContext.Session.SetString("Token", result.Data);
                        HttpContext.Session.SetString("Role", "Admin");
                        return RedirectToPage("/Admin/Index");
                    }
                }
				else
				{
					ViewData["Message"] = result.Message;
					return Page();
				}
			}
			return Page();

		}


		public async Task<IActionResult> OnPostRegister()
		{

			var json = JsonSerializer.Serialize(createAccountRequest);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync("https://localhost:7168/api/Account/CreateAccount", content);
			var data = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<ServiceResponse<AccountResponse>>(data);
			if (result.Success == false)
			{
				ViewData["Notification"] = result.Message;
				return Page();
			}
			else
			{
				ViewData["Notification"] = result.Message;
				return RedirectToPage("/LoginPage");
			}
		}
        private string GetUsernameFromJwt(string jwtToken)
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
            string userName = jwtTokenDecoded.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;

            return userName;
        }
        private string GetAvatarFromJwt(string jwtToken)
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
            string ava = jwtTokenDecoded.Claims.FirstOrDefault(x => x.Type == "Avatar")?.Value;

            return ava;
        }
        private string GetRoleFromJwt(string jwtToken)
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
            var roleClaim = jwtTokenDecoded.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            return roleClaim;
        }
    }
}
