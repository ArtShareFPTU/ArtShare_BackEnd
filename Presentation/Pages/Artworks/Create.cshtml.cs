using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLayer;
using ModelLayer.BussinessObject;
using Newtonsoft.Json;

namespace Presentation.Pages.Artworks
{
    public class CreateModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _artworkManage = "https://localhost:44365/api/Artwork/";
        private readonly string _accountManage = "https://localhost:44365/api/Account";

        public List<Account> Accounts { get; set; }
        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            Accounts = await GetAccounts(client);
            ViewData["AccountId"] = new SelectList(Accounts, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var endpoint = _artworkManage + "CreateArtwork/create";

            var artworkData = new
            {
                accountId = Guid.Parse(Request.Form["Artwork.AccountId"]),
                title = Request.Form["Artwork.Title"],
                description = Request.Form["Artwork.Description"],
                url = Request.Form["Artwork.Url"],
                likes = int.Parse(Request.Form["Artwork.Likes"]),
                fee = decimal.Parse(Request.Form["Artwork.Fee"]),
                status = Request.Form["Artwork.Status"]
            };

            var jsonData = System.Text.Json.JsonSerializer.Serialize(artworkData);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if(response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    TempData["AnnounceMessage"] = "Artwork created success";
                }else if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["AnnounceMessage"] = "This artwork is existed";
                }
                else
                {
                    TempData["AnnounceMessage"] = "Error when creating artwork";
                }
                ModelState.Clear();
                return RedirectToPage();
            }
            return Page();
        }

        private async Task<List<Account>> GetAccounts(HttpClient client)
        {
            var endpoint = _accountManage + "GetAccount";
            var response = await client.GetAsync(endpoint);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Account>>(content);

                return result;
            }
            return null;
        }
    }
}
