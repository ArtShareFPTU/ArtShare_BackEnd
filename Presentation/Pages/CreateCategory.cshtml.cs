using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ModelLayer.DTOS.Request.Category;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Presentation.Pages
{
    public class CreateCategoryModel : PageModel
    {
        private readonly string _categoryManage = "https://localhost:7168/api/Category/";
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty] public CategoryCreation Category { get; set; }

        public CreateCategoryModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            var client = _httpClientFactory.CreateClient();
            var key = HttpContext.Session.GetString("Token");
            if (key == null || key.Length == 0) return RedirectToPage("./Logout");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var key = HttpContext.Session.GetString("Token");
                if (key == null || key.Length == 0) return RedirectToPage("./Logout");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent(Category.Title), "Title");

                var response = await client.PostAsync(_categoryManage + "CreateCategory/create", multipartContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        TempData["AnnounceMessage"] = "Category created successfully";
                        return Page();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        TempData["AnnounceMessage"] = "This category already exists";
                        return Page();
                    }
                }

                TempData["AnnounceMessage"] = "Error when creating category";
                return Page();
            }
            catch (Exception ex)
            {
                TempData["AnnounceMessage"] = $"Error: {ex.Message}";
                return Page();
            }
        }
    }
}