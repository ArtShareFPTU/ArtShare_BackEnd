using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using ModelLayer.BussinessObject;
using Newtonsoft.Json;
using System.Net;

namespace Presentation.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _categoryManage = "https://localhost:44365/api/Category/";

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
      public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var category = await GetCategory(client, id);

            if (category == null)
            {
                return NotFound();
            }
            else 
            {
                Category = category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var result = await DeleteCategory(client, id);

            TempData["AnnounceMessage"] = result;
            return Page();
        }
        private async Task<Category> GetCategory(HttpClient client, Guid id)
        {
            var endpoint = _categoryManage + "GetCategoryById" + id;
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Category>(content);

                return result;
            }
            return null;
        }
        private async Task<string> DeleteCategory(HttpClient client, Guid id)
        {
            var endpoint = _categoryManage + "RemoveCategory/remove" + id;
            var response = await client.PostAsync(endpoint, null);

            string announce = "";
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    announce = "Category has been deleted";
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                announce = "Category is not found";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                announce = "You do not have access";
            }
            return announce;

        }
    }
}
