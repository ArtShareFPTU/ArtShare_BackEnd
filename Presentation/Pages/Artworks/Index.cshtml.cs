﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using ModelLayer.BussinessObject;
using Newtonsoft.Json;

namespace Presentation.Pages.Artworks
{
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _artworkManage = "https://localhost:44365/api/Artwork/";

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Artwork> Artwork { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            //var key = HttpContext.Session.GetString("key");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var artworks = await GetArtworks(client);
            if(artworks == null)
            {
                return NotFound();
            }
            else
            {
                Artwork = artworks;
            }
            return Page();
        }

        private async Task<List<Artwork>> GetArtworks(HttpClient client)
        {
            var endpoint = _artworkManage + "GetArtworks";
            var response = await client.GetAsync(endpoint);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Artwork>>(content);

                return result;
            }
            return null;
        }
    }
}
