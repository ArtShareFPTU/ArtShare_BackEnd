using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLayer;
using ModelLayer.BussinessObject;
using System.Text;
using System.Text.Json;
using ModelLayer.DTOS.Request.Category;
using System.Net.Http;
using System.Net;

namespace Presentation.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _categoryManage = "https://localhost:44365/api/Category/";

        public CategoryCreation Category {  get; set; }

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult OnGet()
        {
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var endpoint = _categoryManage + "CreateCategory/create";


            var title = Request.Form["Category.Title"];
            var multipartData = new MultipartFormDataContent
            {
                { new StringContent(title.ToString()), "Title" }
            };

            var response = await client.PostAsync(endpoint, multipartData);
            if (response.StatusCode != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    TempData["AnnounceMessage"] = "Category created success";
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["AnnounceMessage"] = "This category is existed";
                    return RedirectToPage();
                }
                else
                {
                    TempData["AnnounceMessage"] = "Error when creating, please try again";
                    return RedirectToPage();
                }
                ModelState.Clear();
                return RedirectToPage();
            }
            return Page();
        }
    }
}
