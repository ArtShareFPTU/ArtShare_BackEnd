using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using ModelLayer.DTOS.Response.Account;
using Newtonsoft.Json;
using System.Net;

namespace Presentation.Pages;

public class CreateArtworkModel : PageModel
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _artworkManage = "https://localhost:7168/api/Artwork/";
    private readonly string _accountManage = "https://localhost:7168/api/Account/";
    private readonly string _tagManage = "https://localhost:7168/api/Tag/";
    private readonly string _categoryManage = "https://localhost:7168/api/Category/";


    public AccountResponse Accounts { get; set; }
    public ArtworkCreation Artwork { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Category> Categories { get; set; }

    public CreateArtworkModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGet()
    {
        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("Token");
        if (key == null || key.Length == 0) return RedirectToPage("./LogoutPage");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        Accounts = await GetAccounts(client);
        Tags = await GetTag(client);
        Categories = await GetCategory(client);
        //ViewData["AccountId"] = new SelectList(Accounts.Select(c => c.Id), "AccountId", "AccountId");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpClientFactory.CreateClient();
        var key = HttpContext.Session.GetString("key");
        if (key == null) return RedirectToPage("./LogoutPage");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        var endpoint = _artworkManage + "CreateArtwork/create";

        // Create multipart form data content
        var multipartContent = new MultipartFormDataContent();

        // Add artwork data as JSON string

        var artworkData = new ArtworkCreation
        {
            AccountId = Guid.Parse(Request.Form["Artwork.AccountId"]),
            Title = Request.Form["Artwork.Title"],
            Description = Request.Form["Artwork.Description"],
            Fee = decimal.Parse(Request.Form["Artwork.Fee"]),
            ArtworkCategories = Request.Form["Artwork.ArtworkCategories"].Select(id => Guid.Parse(id)).ToList(),
            ArtworkTags = Request.Form["Artwork.ArtworkTags"].Select(id => Guid.Parse(id)).ToList()
        };

        multipartContent.Add(new StringContent(artworkData.AccountId.ToString()), "AccountId");
        multipartContent.Add(new StringContent(artworkData.Title), "Title");
        multipartContent.Add(new StringContent(artworkData.Description), "Description");
        multipartContent.Add(new StringContent("0"), "Likes");
        multipartContent.Add(new StringContent(artworkData.Fee.ToString()), "Fee");
        multipartContent.Add(new StringContent("Active"), "Status");
        foreach (var categoryId in artworkData.ArtworkCategories)
        {
            multipartContent.Add(new StringContent(categoryId.ToString()), "ArtworkCategories");
        }

        foreach (var tagId in artworkData.ArtworkTags)
        {
            multipartContent.Add(new StringContent(tagId.ToString()), "ArtworkTags");
        }
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
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("./LogoutPage");
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

    private async Task<AccountResponse> GetAccounts(HttpClient client)
    {
        var endpoint = _accountManage + "GetCurrentAccount";
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountResponse>(content);

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