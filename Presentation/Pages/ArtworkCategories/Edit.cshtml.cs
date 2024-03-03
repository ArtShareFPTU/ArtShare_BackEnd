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
using ModelLayer.DTOS.Request.Artwork;
using System.Net;

namespace Presentation.Pages.ArtworkCategories
{
    public class EditModel : PageModel
    {
        private readonly string _artworkManage = "https://localhost:7168/api/Artwork/";
        private readonly string _tagManage = "https://localhost:7168/api/Tag/";
        private readonly string _categoryManage = "https://localhost:7168/api/Category/";
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Tag> Tags { get; set; }
        public List<Category> Categories { get; set; }
        public List<ArtworkCategory> ArtworkCategories { get; set; }
        public List<Artwork> Artworks { get; set; }

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ArtworkCategory ArtworkCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient();
            Tags = await GetTag(client);
            Categories = await GetCategory(client);
            ArtworkCategories = await GetArtworkCategory(client);
            Artworks = await GetArtworks(client);
            var artworkcategory = ArtworkCategories.FirstOrDefault(c => c.Id.Equals(id));

            if (artworkcategory == null)
            {
                return NotFound();
            }
            else
            {
                ArtworkCategory = artworkcategory;
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
            var endpoint = _artworkManage + "UpdateCategory4Artwork/updateCategory";

            var artworkCategoryData = new ArtworkCategoryUpdate
            {
                Id = Guid.Parse(Request.Form["ArtworkCategory.Id"]),
                ArtworkId = Guid.Parse(Request.Form["ArtworkCategory.ArtworkId"]),
                CategoryId = Guid.Parse(Request.Form["ArtworkCategory.CategoryId"])
            };

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(artworkCategoryData.Id.ToString()), "Id");
            multipartContent.Add(new StringContent(artworkCategoryData.ArtworkId.ToString()), "ArtworkId");
            multipartContent.Add(new StringContent(artworkCategoryData.CategoryId.ToString()), "CategoryId");

            var response = await client.PostAsync(endpoint, multipartContent);
            if (response.StatusCode != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    TempData["AnnounceMessage"] = "Update category for Artwork is finished";
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["AnnounceMessage"] = "This category is removed";
                    return RedirectToPage();
                }
                else
                {
                    TempData["AnnounceMessage"] = "Error when updating category for artwork";
                    return RedirectToPage();
                }

                ModelState.Clear();
                return RedirectToPage();
            }
            return Page();
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
        public async Task<List<ArtworkCategory>> GetArtworkCategory(HttpClient client)
        {
            var endpoint = _artworkManage + "GetArtworkCategories";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ArtworkCategory>>(content);

                return categories;
            }

            return null;
        }
        private async Task<List<Artwork>> GetArtworks(HttpClient client)
        {
            var endpoint = _artworkManage + "GetArtworks";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Artwork>>(content);

                return result;
            }

            return null;
        }
    }
}
