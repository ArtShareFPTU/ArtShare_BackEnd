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
    }
}
