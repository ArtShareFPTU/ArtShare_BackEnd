using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;

namespace Presentation.Pages.Admin
{
    public class ManageArtworkModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _adminManage = "https://localhost:7168/api/";

        public ManageArtworkModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty] public IEnumerable<Artwork> artworks { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var art = await GetArtworks(client);
            if (art == null)
            {
                return NotFound();
            }
            else
            {
                artworks = art;
            }
            return Page();
        }
        private async Task<IEnumerable<Artwork>> GetArtworks(HttpClient client)
        {
            var endpoint = _adminManage + "Artwork/GetArtworksForAdmin";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Artwork>>(content);

                return result;
            }
            return null;
        }
    }
}
