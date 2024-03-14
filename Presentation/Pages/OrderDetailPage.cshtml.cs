using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response;
using Newtonsoft.Json;
using System.Net.Http;

namespace Presentation.Pages
{
    
    public class OrderDetailPageModel : PageModel
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Order Order { get; set; }=  default!;
        [BindProperty]
        public List<Artwork> Artwork { get; set; } = default!;
        public OrderDetailPageModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task OnGetAsync(Guid id)
        {
            var order = await _client.GetAsync($"https://localhost:7168/api/Order/GetOrderById/{id}");
            if (order.IsSuccessStatusCode)
            {
                var jsonString = await order.Content.ReadAsStringAsync();

                Order = JsonConvert.DeserializeObject<Order>(jsonString);
            }
            var artwork = await _client.GetAsync($"https://localhost:7168/api/Order/GetArtWorkByOderId/{id}");
            if(artwork.IsSuccessStatusCode)
            {
                var jsonString = await artwork.Content.ReadAsStringAsync();
                Artwork = JsonConvert.DeserializeObject<List<Artwork>>(jsonString);
            }

        }
        
        public async Task<IActionResult> OnPostDownload(Guid id)
        {
            var key = HttpContext.Session.GetString("Token");
            if (key == null || key.Length == 0) return RedirectToPage("./Logout");
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            string url = "https://localhost:7168/api/Artwork/DownloadImage?id=" + id;
            var downloadImage = await _client.GetAsync(url);
            if (downloadImage.IsSuccessStatusCode)
            {
                var fileName = downloadImage.Content.Headers.ContentDisposition.FileName;
                fileName = RemovePaidVersionFromFileName(fileName);
                var fileData = await downloadImage.Content.ReadAsByteArrayAsync();
                return File(fileData, "image/jpg", fileName);
            }
            return Page();
        }

        private string RemovePaidVersionFromFileName(string fileName)
        {
            const string paidVersion = "-Paid-version";

            int paidVersionIndex = fileName.IndexOf(paidVersion);
            if (paidVersionIndex != -1)
            {
                // Loại bỏ phần "Paid version" và khoảng trắng phía sau nếu có
                fileName = fileName.Remove(paidVersionIndex, paidVersion.Length).TrimEnd();
            }

            return fileName;
        }
    }
}
