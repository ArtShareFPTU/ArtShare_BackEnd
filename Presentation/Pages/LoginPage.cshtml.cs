using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Response.Commons;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Presentation.Pages
{
    public class LoginPageModel : PageModel
    {
		private readonly HttpClient _client = new HttpClient();

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
				var data = await  response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<ServiceResponse<string>>(data);
				HttpContext.Session.SetString("Token", result.Data);
				return RedirectToPage("/HomePage");
			}
			return Page();

		}


		public IActionResult OnPostRegister()
		{

			var json = JsonSerializer.Serialize(createAccountRequest);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = _client.PostAsync("https://localhost:7168/api/Account/CreateAccount", content).Result;
			if (response.IsSuccessStatusCode)
			{
				var data = response.Content.ReadAsStringAsync();
				//




				return RedirectToPage("/HomePage");
			}
			return Page();

		}
	}
}
