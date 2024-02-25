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
    public class DetailsModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _tagManage = "https://localhost:44365/api/tag/";

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null) return NotFound();
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var tag = await GetTag(client, id);
            if(tag == null) return NotFound();
            return Page();
        }

        public async Task<Tag> GetTag(HttpClient client, Guid id)
        {
            var endpoint = _tagManage + "GetTagById/" + id;
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tag = JsonConvert.DeserializeObject<Tag>(content);

                return tag;
            }
            return null;
        }
    }
}
