using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Artwork
{
    public class ArtworkTagUpdate
    {
        public Guid Id { get; set; }
        public Guid ArtworkId { get; set; }
        public Guid TagId { get; set; }
    }
}
