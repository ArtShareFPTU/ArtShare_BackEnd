using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Account
{
    public class LoginAccountResponse
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
