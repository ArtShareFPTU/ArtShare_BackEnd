using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Artwork
{
    public class ArtworkCategoryAddition
    {
        public Guid ArtworkId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
