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

namespace Presentation.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _tagManage = "https://localhost:44365/api/tag/";


        public List<Tag> Tag { get;set; } = default!;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var tags = await GetAllTag(client);

            if(tags == null)
            {
                return NotFound();
            }
            else
            {
                Tag = tags;
            }
            return Page();

        }

        private async Task<List<Tag>> GetAllTag(HttpClient client)
        {
            var endpoint = _tagManage + "GetTags";
            var response = await client.GetAsync(endpoint);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Tag>>(content);

                Tag = result;
            }
            else
            {

            }
            return Tag;
        }
        
    }
}
