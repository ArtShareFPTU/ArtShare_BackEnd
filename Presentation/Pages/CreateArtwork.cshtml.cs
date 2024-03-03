using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using Newtonsoft.Json;
using System.Net;

namespace Presentation.Pages;

public class CreateArtworkModel : PageModel
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _artworkManage = "https://localhost:44365/api/Artwork/";
    private readonly string _accountManage = "https://localhost:44365/api/Account/";

    public List<Account> Accounts { get; set; }
    public ArtworkCreation Artwork { get; set; }

    public CreateArtworkModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGet()
    {
        var client = _httpClientFactory.CreateClient();
        //var key = HttpContext.Session.GetString("key");
        //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        Accounts = await GetAccounts(client);

        //ViewData["AccountId"] = new SelectList(Accounts.Select(c => c.Id), "AccountId", "AccountId");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpClientFactory.CreateClient();
        var endpoint = _artworkManage + "CreateArtwork/create";

        // Create multipart form data content
        var multipartContent = new MultipartFormDataContent();

        // Add artwork data as JSON string

        var artworkData = new
        {
            accountId = Guid.Parse(Request.Form["Artwork.AccountId"]),
            title = Request.Form["Artwork.Title"],
            description = Request.Form["Artwork.Description"],
            likes = int.Parse(Request.Form["Artwork.Likes"]),
            fee = decimal.Parse(Request.Form["Artwork.Fee"]),
            status = Request.Form["Artwork.Status"]
        };

        multipartContent.Add(new StringContent(artworkData.accountId.ToString()), "AccountId");
        multipartContent.Add(new StringContent(artworkData.title), "Title");
        multipartContent.Add(new StringContent(artworkData.description), "Description");
        multipartContent.Add(new StringContent(artworkData.likes.ToString()), "Likes");
        multipartContent.Add(new StringContent(artworkData.fee.ToString()), "Fee");
        multipartContent.Add(new StringContent(artworkData.status), "Status");
        if (Request.Form.Files.Count > 0)
        {
            var imageFile = Request.Form.Files[0];
            multipartContent.Add(new StreamContent(imageFile.OpenReadStream()), "Image", imageFile.FileName);
        }
        else
        {
            //multipartContent.Add(new StringContent(string.Empty), "Image");
        }

        var response = await client.PostAsync(endpoint, multipartContent);
        if (response.StatusCode != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Created)
            {
                TempData["AnnounceMessage"] = "Artwork created success";
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                TempData["AnnounceMessage"] = "This artwork is existed";
                return RedirectToPage();
            }
            else if (response.StatusCode == HttpStatusCode.UnsupportedMediaType)
            {
                TempData["AnnounceMessage"] = "This file is not supported, try another one";
                return RedirectToPage();
            }
            else
            {
                TempData["AnnounceMessage"] = "Error when creating artwork";
                return RedirectToPage();
            }

            ModelState.Clear();
            return RedirectToPage();
        }

        return Page();
    }

    private async Task<List<Account>> GetAccounts(HttpClient client)
    {
        var endpoint = _accountManage + "GetAccount";
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Account>>(content);

            return result;
        }

        return null;
    }
}