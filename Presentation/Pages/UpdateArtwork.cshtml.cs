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
    private readonly string _artworkManage = "https://localhost:7168/api/Artwork/";
    private readonly string _tagManage = "https://localhost:7168/api/Tag/";
    private readonly string _categoryManage = "https://localhost:7168/api/Category/";

    public UpdateArtworkModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty] public Artwork Artwork { get; set; } = default!;
    public List<Tag> Tags { get; set; }
    public List<Category> Categories { get; set; }

    public ArtworkUpdate ArtworkUpdate { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if (id == null) return NotFound();
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("Token");
        if (key == null || key.Length == 0) return RedirectToPage("./Logout");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artwork = await GetArtwork(client, id);
        Tags = await GetTag(client);
        Categories = await GetCategory(client);

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
        var key = HttpContext.Session.GetString("Token");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var artworkUpdate = new ArtworkUpdate
        {
            Id = Guid.Parse(Request.Form["ArtworkUpdate.Id"]),
            AccountId = Guid.Parse(Request.Form["ArtworkUpdate.AccountId"]),
            Title = Request.Form["ArtworkUpdate.Title"],
            Description = Request.Form["ArtworkUpdate.Description"],
            Fee = decimal.Parse(Request.Form["ArtworkUpdate.Fee"]),
            Status = Request.Form["ArtworkUpdate.Status"],
            ArtworkCategories = Request.Form["Artwork.ArtworkCategories"].Where(id => !string.IsNullOrEmpty(id)).Select(Guid.Parse).ToList(),
            ArtworkTags = Request.Form["Artwork.ArtworkTags"].Where(id => !string.IsNullOrEmpty(id)).Select(Guid.Parse).ToList()
        };

        var endpoint = _artworkManage + "UpdateArtwork/update";

        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(new StringContent(artworkUpdate.Id.ToString()), "Id");
        multipartContent.Add(new StringContent(artworkUpdate.AccountId.ToString()), "AccountId");
        multipartContent.Add(new StringContent(artworkUpdate.Title), "Title");
        multipartContent.Add(new StringContent(artworkUpdate.Description), "Description");
        multipartContent.Add(new StringContent(artworkUpdate.Fee.ToString()), "Fee");
        multipartContent.Add(new StringContent(artworkUpdate.Status), "Status");
        foreach (var categoryId in artworkUpdate.ArtworkCategories)
        {
            multipartContent.Add(new StringContent(categoryId.ToString()), "ArtworkCategories");
        }

        foreach (var tagId in artworkUpdate.ArtworkTags)
        {
            multipartContent.Add(new StringContent(tagId.ToString()), "ArtworkTags");
        }

        if (Request.Form.Files.Count > 0)
        {
            var imageFile = Request.Form.Files[0];
            multipartContent.Add(new StreamContent(imageFile.OpenReadStream()), "Image", imageFile.FileName);
        }

        var response = await client.PutAsync(endpoint, multipartContent);
        if (response.StatusCode != null)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["AnnounceMessage"] = "Update artwork success";
                return RedirectToPage("/UpdateArtwork", new { id = artworkUpdate.Id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                TempData["AnnounceMessage"] = "This artwork was removed or not existed before";
                return RedirectToPage("/UpdateArtwork", new { id = artworkUpdate.Id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("./Logout");
            }
            else
            {
                TempData["AnnounceMessage"] = "Error when updating artwork";
                return RedirectToPage("/UpdateArtwork", new { id = artworkUpdate.Id });
            }
        }

        ModelState.Clear();
        return RedirectToPage("/UpdateArtwork", new { id = artworkUpdate.Id });
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
}