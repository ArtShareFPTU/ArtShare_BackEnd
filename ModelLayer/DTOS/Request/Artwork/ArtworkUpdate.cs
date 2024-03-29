﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Artwork;

public class ArtworkUpdate
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public int? Likes { get; set; }
    public decimal? Fee { get; set; }
    public string? Status { get; set; }
    public List<Guid> ArtworkCategories { get; set; }
    public List<Guid> ArtworkTags { get; set; }
}