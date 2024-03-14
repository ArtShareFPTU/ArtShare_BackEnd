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

namespace Presentation.Pages.ArtworkTags
{
    public class DeleteModel : PageModel
    {
        private readonly string _artworkManage = "https://localhost:7168/api/Artwork/";
        private readonly string _tagManage = "https://localhost:7168/api/Tag/";
        private readonly string _categoryManage = "https://localhost:7168/api/Category/";
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Tag> Tags { get; set; }
        public List<Category> Categories { get; set; }
        public List<ArtworkTag> ArtworkTags { get; set; }

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty] public ArtworkTag ArtworkTag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            Tags = await GetTag(client);
            Categories = await GetCategory(client);
            ArtworkTags = await GetArtworkTag(client);
            var artworkcategory = ArtworkTags.FirstOrDefault(c => c.Id.Equals(id));

            if (artworkcategory == null)
            {
                return NotFound();
            }
            else
            {
                ArtworkTag = artworkcategory;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var result = await DeleteArtworkTag(client, (Guid)id);
            TempData["AnnounceMessage"] = result;
            return Page();
        }

        private async Task<string> DeleteArtworkTag(HttpClient client, Guid id)
        {
            var endpoint = _artworkManage + "RemoveTag4Artwork/removeTag/" + id;
            var response = await client.PostAsync(endpoint, null);

            var announce = "";

            if (response.StatusCode == HttpStatusCode.OK) announce = "Artwork tag has been removed";
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                announce = "Artwork is not found";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                announce = "You do not have access";
            }

            return announce;
        }

        public async Task<List<Tag>> GetTag(HttpClient client)
        {
            var endpoint = _tagManage + "GetTags";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tag = JsonConvert.DeserializeObject<List<Tag>>(content);

                return tag;
            }

            return null;
        }

        public async Task<List<Category>> GetCategory(HttpClient client)
        {
            var endpoint = _categoryManage + "GetCategorys";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(content);

                return categories;
            }

            return null;
        }

        public async Task<List<ArtworkTag>> GetArtworkTag(HttpClient client)
        {
            var endpoint = _artworkManage + "GetArtworkTags";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tags = JsonConvert.DeserializeObject<List<ArtworkTag>>(content);

                return tags;
            }

            return null;
        }
    }
}