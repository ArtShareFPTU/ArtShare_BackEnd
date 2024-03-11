using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Like
{
    public class LikeCreation
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? ArtworkId { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
