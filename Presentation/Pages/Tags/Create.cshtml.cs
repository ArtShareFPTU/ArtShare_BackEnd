using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLayer;
using ModelLayer.BussinessObject;
using System.Text.Json;
using System.Text;
using ModelLayer.DTOS.Request.Tags;

namespace Presentation.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _tagManage = "https://localhost:44365/api/Tag/";

        public TagCreation Tag {  get; set; }

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();  
            }
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var endpoint = _tagManage + "CreateTag/create";

           
            var title = Request.Form["Tag.Title"];
            var requestData = new { title = title };
            var multipartContent = new MultipartFormDataContent
            {
                { new StringContent(title), "title" }
            };

            var response = await client.PostAsync(endpoint, multipartContent);
            if (response.StatusCode != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if(response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    TempData["AnnounceMessage"] = "Tag created success";
                }else if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["AnnounceMessage"] = "This tag is existed";
                }
                else
                {
                    TempData["AnnounceMessage"] = "Error when creating, please try again";
                }

                ModelState.Clear();
                return RedirectToPage();
            }
            return Page();
        }
    }
}
