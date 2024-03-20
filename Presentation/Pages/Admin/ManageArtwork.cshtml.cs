using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Pagination;
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
        [BindProperty] public Pagination<Artwork> artworks { get; set; }
        [BindProperty] public int PageIndex { get; set; } = 0;
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            if(pageIndex == null)
            {
                var art = await GetArtworks(PageIndex, client);
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
            else
            {
                var art = await GetArtworks(pageIndex, client);
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
            
        }
        private async Task<Pagination<Artwork>> GetArtworks(int? pageIndex,HttpClient client)
        {
            var endpoint = _adminManage + $"Artwork/GetArtworksForAdmin/{pageIndex}";
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Pagination<Artwork>>(content);

                return result;
            }
            return null;
        }
    }
}
