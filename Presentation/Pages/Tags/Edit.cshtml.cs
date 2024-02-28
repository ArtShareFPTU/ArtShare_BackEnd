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
using ModelLayer.DTOS.Request.Tags;
using System.Text.Json;

namespace Presentation.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _tagManage = "https://localhost:44365/api/tag/";

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null) return NotFound();
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var tag = await GetTag(client, id);
            if (tag == null) return NotFound();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var tagUpdate = new TagUpdate
            {
                Id = Guid.Parse(Request.Form["Tag.Id"]),
                Title = Request.Form["Tag.Title"]
            };

            var endpoint = _tagManage + "UpdateTag/update";
            var jsonRequestData = System.Text.Json.JsonSerializer.Serialize(tagUpdate);

            var content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endpoint, content);
            if (response.IsSuccessStatusCode)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["AnnounceMessage"] = "Update tag success";
                }else if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["AnnounceMessage"] = "This tag was removed or not existed before";
                }
                else
                {
                    TempData["AnnounceMessage"] = "Error when updating tag";
                }
            }
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
