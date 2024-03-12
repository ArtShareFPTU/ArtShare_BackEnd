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
    public class DetailsModel : PageModel
    {
        private readonly DataAccessLayer.ArtShareContext _context;

        public DetailsModel(DataAccessLayer.ArtShareContext context)
        {
            _context = context;
        }

        public Artwork Artwork { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks.FirstOrDefaultAsync(m => m.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }
            else
            {
                Artwork = artwork;
            }

            return Page();
        }
    }
}