﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Account;

public class CreateAccountRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Description { get; set; }
    public string? FullName { get; set; }
    public DateTime? Birthday { get; set; }
    public string? UserName { get; set; }
}