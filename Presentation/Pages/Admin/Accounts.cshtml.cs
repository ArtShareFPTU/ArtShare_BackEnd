using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;

namespace Presentation.Pages.Admin
{
    public class AccountsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _adminManage = "https://localhost:7168/api/";
        private readonly IConfiguration _configuration;

        [BindProperty] public List<AccountResponse> Accounts { get; set; }
        public AccountsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var account = await GetAccounts(client);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                Accounts = account;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var del = await DeleteAccount(id,client);
            if(del == true)
            {
                var account = await GetAccounts(client);
                Accounts = account;
                return BadRequest("Delete unsuccessfully");
            }
            else
            {
                var account = await GetAccounts(client);
                Accounts = account;
                return Page();
            }
                
        }
        private async Task<bool> DeleteAccount(Guid id, HttpClient client)
        {
            var endpoint = _adminManage + $"Account/DeleteAccount/{id}";
            var response = await client.DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                return true;
            }else
            return false;
        }
        private async Task<List<AccountResponse>> GetAccounts(HttpClient client)
        {
            var endpoint = _adminManage + "Account/GetAccount";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<AccountResponse>>(content);

                return result;
            }
            return null;
        }
    }
}