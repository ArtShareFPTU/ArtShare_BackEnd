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

namespace Presentation.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _categoryManage = "https://localhost:44365/api/Category/";

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Category> Category { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var categorys = await GetCategories(client);

            if(categorys == null)
            {
                return NotFound();
            }
            else
            {
                Category = categorys;
            }
            return Page();
        }

        private async Task<List<Category>> GetCategories(HttpClient client)
        {
            var endpoint = _categoryManage + "GetCategorys";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Category>>(content);

                return result;
            }
            return null;
        }
    }
}
