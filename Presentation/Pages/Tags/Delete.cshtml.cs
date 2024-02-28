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

namespace Presentation.Pages.Tags
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _tagManage = "https://localhost:44365/api/tag/";

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if(id == null) return NotFound();
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var tagToShow = await GetTag(client, id);

            if(tagToShow == null)
            {
                return NotFound();
            }
            else
            {
                Tag = tagToShow;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null) return NotFound();
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var result = await DeleteTag(client, id);

            TempData["AnnounceMessage"] = result;
            return Page();
        }

        public async Task<Tag> GetTag(HttpClient client, Guid id)
        {
            var endpoint = _tagManage + "GetTagById/" + id;
            var response = await client.GetAsync(endpoint);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tag = JsonConvert.DeserializeObject<Tag>(content);

                return tag;
            }
            return null;
        }
        private async Task<string> DeleteTag(HttpClient client, Guid? id)
        {
            var endpoint = _tagManage + "RemoveTag/remove/" + id;
            var response = await client.PostAsync(endpoint, null);
            string announce = "";
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    announce = "Tag has been deleted";
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                announce = "Tag is not found";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                announce = "You do not have access";
            }
            return announce;
        }
    }
}
