using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using ModelLayer.BussinessObject;
using Newtonsoft.Json;
using ModelLayer.DTOS.Response.Account;
using ModelLayer.DTOS.Request.Account;
using System.Text;
using ModelLayer.DTOS.Request.Artwork;

namespace Presentation.Pages.Shared
{
    public class EditProfilePageModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _accountManage = "https://localhost:7168/api/Account/";
        private readonly IConfiguration _configuration;

        public EditProfilePageModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty] public AccountResponse Account { get; set; }
        [BindProperty] public UpdateAccountRequest UpdateAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var account = await GetAccount(id, client);
            if (account == null)
            {
                return NotFound();
            }

            Account = account;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var account = await GetAccount(Account.Id, client);
            if (account == null)
            {
                return NotFound();
            }

            await Update(Account.Id, client);

            return RedirectToPage("ProfilePage");
        }

        private async Task<AccountResponse> GetAccount(Guid id, HttpClient client)
        {
            var endpoint = _accountManage + $"GetAccount/{id}";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AccountResponse>(content);

                return result;
            }

            return null;
        }

        public async Task<AccountResponse> Update(Guid id, HttpClient client)
        {
            var json = JsonConvert.SerializeObject(UpdateAccount);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = _accountManage + $"UpdateAccount/{id}";
            // Create multipart form data content
            var multipartContent = new MultipartFormDataContent();

            // Add artwork data as JSON string

            /*var accountData = new UpdateAccountRequest
            {
                FullName = UpdateAccount.FullName,
                UserName = UpdateAccount.UserName,
                Description = UpdateAccount.Description,
            };
            multipartContent.Add(new StringContent(accountData.FullName), "FullName");
            multipartContent.Add(new StringContent(accountData.UserName), "UserName");
            multipartContent.Add(new StringContent(accountData.Description), "Description");*/
            /*if (Request.Form.Files.Count > 0)
            {
                var imageFile = Request.Form.Files[0];
                multipartContent.Add(new StreamContent(imageFile.OpenReadStream()), "Avatar", imageFile.FileName);
            }*/
            var response = await client.PutAsync(endpoint, content /*multipartContent*/);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AccountResponse>(data);
                if (result.Avatar != null)
                {
                    HttpContext.Session.SetString("Avatar", result.Avatar);
                }

                return result;
            }

            return null;
        }
    }
}