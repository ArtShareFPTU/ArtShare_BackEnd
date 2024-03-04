using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ArtworkTagRepository : IArtworkTagRepository
    {
        private readonly ArtShareContext _context;

        public ArtworkTagRepository()
        {
            _context = new ArtShareContext();
        }

        public async Task<List<ArtworkTag>> GetAllTagFromSearch(string search)
        {
            var checklist = await _context.Set<ArtworkTag>().Include(c => c.Tag).Where(c => c.Tag.Title.Contains(search)).ToListAsync();
            return checklist;
        }
    }
}
