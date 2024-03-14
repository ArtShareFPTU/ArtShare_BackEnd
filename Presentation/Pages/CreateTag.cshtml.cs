using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.DTOS.Request.Tags;

namespace Presentation.Pages
{
    public class CreateTagModel : PageModel
    {
        private readonly string _tagManage = "https://localhost:7168/api/Tag/";
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public TagCreation Tag { get; set; }

        public CreateTagModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            if (key == null || key.Length == 0) return RedirectToPage("./Logout");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var key = HttpContext.Session.GetString("Token");
                if (key == null || key.Length == 0) return RedirectToPage("./Logout");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent(Request.Form["Tag.Title"]), "Title");

                var response = await client.PostAsync(_tagManage + "CreateTag/create", multipartContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        TempData["AnnounceMessage"] = "Tag created successfully";
                        return Page();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        TempData["AnnounceMessage"] = "This tag is already existed";
                        return Page();
                    }
                }

                TempData["AnnounceMessage"] = "Error when creating tag";
                return Page();
            }
            catch (Exception ex)
            {
                TempData["AnnounceMessage"] = ex.Message;
                return Page();
            }
        }

    }
}
