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
    public class DeleteModel : PageModel
    {
        private readonly DataAccessLayer.ArtShareContext _context;

        public DeleteModel(DataAccessLayer.ArtShareContext context)
        {
            _context = context;
        }

        [BindProperty] public Artwork Artwork { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks.FindAsync(id);

            if (artwork != null)
            {
                Artwork = artwork;
                _context.Artworks.Remove(Artwork);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}