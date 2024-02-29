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
using ModelLayer.DTOS.Request.Category;
using ModelLayer.DTOS.Request.Artwork;

namespace Presentation.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _categoryManage = "https://localhost:44365/api/Category/";

        public EditModel(IHttpClientFactory httpClientFactory)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var categoryUpdate = new CategoryUpdate
            {
                Id = Guid.Parse(Request.Form["Category.Id"]),
                Title = Request.Form["Category.Title"]
            };
             var endpoint = _categoryManage + "UpdateCategory/update";
            var multipartContent = new MultipartFormDataContent
            {
                { new StringContent(categoryUpdate.Id.ToString()), "Id" },
                { new StringContent(categoryUpdate.Title), "Title" }
            };

            var response = await client.PutAsync(endpoint, multipartContent);
            if (response.StatusCode != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["AnnounceMessage"] = "Update category success";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["AnnounceMessage"] = "This artwork was removed or not existed before";
                }
                else
                {
                    TempData["AnnounceMessage"] = "Error when updating category";
                }
            }
            return Page();
        }

        private async Task<Category> GetCategory(HttpClient client, Guid id)
        {
            var endpoint = _categoryManage + "GetCategoryById/" + id;
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Category>(content);

                return result;
            }
            return null;
        }
    }
}
