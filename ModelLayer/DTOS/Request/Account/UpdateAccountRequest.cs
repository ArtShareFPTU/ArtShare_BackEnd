using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Account
{
    public class UpdateAccountRequest
    {
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }

    }
}
