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

namespace Presentation.Pages.Artworks
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _artworkManage = "https://localhost:44365/api/Artwork/";

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
      public Artwork Artwork { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var artwork = await GetArtwork(client, id);

            if (artwork == null)
            {
                return NotFound();
            }
            else 
            {
                Artwork = artwork;
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
            var result = await DeleteArtwork(client, id);

            TempData["AnnounceMessage"] = result;
            return Page();
        }

        private async Task<Artwork> GetArtwork(HttpClient client, Guid id)
        {
            var endpoint = _artworkManage + "GetArtworkById/" + id;
            var response = await client.GetAsync(endpoint);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Artwork>(content);

                return result;
            }
            return null;
        }

        private async Task<string> DeleteArtwork(HttpClient client, Guid id)
        {
            var endpoint = _artworkManage + "RemoveArtwork/remove/" + id;
            var response = await client.PostAsync(endpoint, null);

            string announce = "";
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    announce = "Artwork has been disabled";
                }
            }
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
    }
}
