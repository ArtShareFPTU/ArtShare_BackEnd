using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Response.Account
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Status { get; set; }
        public int? NumArtwork { get; set; }
        public int? NumFollowers { get; set; }
        public int? NumFollowings { get; set; }
    }
}
