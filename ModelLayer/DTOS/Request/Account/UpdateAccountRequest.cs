using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Account;

public class UpdateAccountRequest
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string? FullName { get; set; }
    public IFormFile? Avatar { get; set; }
}