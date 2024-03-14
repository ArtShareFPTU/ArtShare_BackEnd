using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using ModelLayer.BussinessObject;

namespace Presentation.Pages.Artworks
{
    public class IndexModel : PageModel
    {
        private readonly DataAccessLayer.ArtShareContext _context;

        public IndexModel(DataAccessLayer.ArtShareContext context)
        {
            _context = context;
        }

        public IList<Artwork> Artwork { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Artworks != null)
            {
                Artwork = await _context.Artworks
                    .Include(a => a.Account).ToListAsync();
            }
        }
    }
}