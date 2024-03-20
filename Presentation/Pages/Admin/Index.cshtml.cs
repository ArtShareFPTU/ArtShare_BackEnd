using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;

namespace Presentation.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _adminManage = "https://localhost:7168/api/";
        [BindProperty] public List<AccountResponse> Accounts { get; set; }
        [BindProperty] public List<Account> AccountsTop5 { get; set; }
        [BindProperty] public List<OrderDetail> orders { get; set; } = default!;
        [BindProperty] public IEnumerable<Artwork> artworks { get; set; }
        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var account = await GetAccounts(client);
            var art = await GetArtworks(client);
            var ord = await GetOrderDetails(client);
            var acctop = await GetTop5Accounts(client);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                Accounts = account;
                artworks = art;
                orders = ord;
                AccountsTop5 = acctop;
            }
            return Page();
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
        private async Task<IEnumerable<Artwork>> GetArtworks(HttpClient client)
        {
            var endpoint = _adminManage + "Artwork/GetArtworks";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Artwork>>(content);

                return result;
            }
            return null;
        }
        private async Task<List<OrderDetail>> GetOrderDetails(HttpClient client)
        {
            var endpoint = _adminManage + "Order/GetOrderDetails";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<OrderDetail>>(content);

                return result;
            }
            return null;
        }
        private async Task<List<Account>> GetTop5Accounts(HttpClient client)
        {
            var endpoint = _adminManage + "Account/GetTop5Account";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Account>>(content);

                return result;
            }
            return null;
        }
    }
}