using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Request.Tags;

public class TagUpdate
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}