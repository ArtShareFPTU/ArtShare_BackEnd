using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Comment
{
    public class CommentCreation
    {

        public Guid? AccountId { get; set; }
        public Guid? ArtworkId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
