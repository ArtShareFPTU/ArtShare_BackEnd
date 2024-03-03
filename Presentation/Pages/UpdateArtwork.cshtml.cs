using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using Newtonsoft.Json;
using System.Net.Http;

namespace Presentation.Pages;

public class UpdateArtworkModel : PageModel
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _artworkManage = "https://localhost:44365/api/Artwork/";

    public UpdateArtworkModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty] public Artwork Artwork { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if (id == null) return NotFound();
        var client = _httpClientFactory.CreateClient();
        //var key = HttpContext.Session.GetString("key");
        //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artwork = await GetArtwork(client, id);

        if (artwork == null)
            Artwork = null;
        else
            Artwork = artwork;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var client = _httpClientFactory.CreateClient();
        //var key = HttpContext.Session.GetString("key");
        //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artworkUpdate = new ArtworkUpdate
        {
            Id = Guid.Parse(Request.Form["Artwork.Id"]),
            AccountId = Guid.Parse(Request.Form["Artwork.AccountId"]),
            Title = Request.Form["Artwork.Title"],
            Description = Request.Form["Artwork.Description"],
            Fee = decimal.Parse(Request.Form["Artwork.Fee"]),
            Status = Request.Form["Artwork.Status"]
        };

        var endpoint = _artworkManage + "UpdateArtwork/update";
        
        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(new StringContent(artworkUpdate.Id.ToString()), "AccountId");
        multipartContent.Add(new StringContent(artworkUpdate.Title), "Title");
        multipartContent.Add(new StringContent(artworkUpdate.Description), "Description");
        multipartContent.Add(new StringContent(artworkUpdate.Fee.ToString()), "Fee");
        multipartContent.Add(new StringContent(artworkUpdate.Status), "Status");
        if (Request.Form.Files.Count > 0)
        {
            var imageFile = Request.Form.Files[0];
            multipartContent.Add(new StreamContent(imageFile.OpenReadStream()), "Image", imageFile.FileName);
        }

        var response = await client.PutAsync(endpoint, multipartContent);
        if (response.StatusCode != null)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                TempData["AnnounceMessage"] = "Update artwork success";
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                TempData["AnnounceMessage"] = "This artwork was removed or not existed before";
            else
                TempData["AnnounceMessage"] = "Error when updating artwork";
        }

        return Page();
    }


    private async Task<Artwork> GetArtwork(HttpClient client, Guid id)
    {
        var endpoint = _artworkManage + "GetArtworkById/" + id;
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Artwork>(content);

            return result;
        }

        return null;
    }
}