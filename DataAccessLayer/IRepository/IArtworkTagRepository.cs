using ModelLayer.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IArtworkTagRepository
    {
        Task<List<ArtworkTag>> GetAllTagFromSearch(string search);
    }
}
