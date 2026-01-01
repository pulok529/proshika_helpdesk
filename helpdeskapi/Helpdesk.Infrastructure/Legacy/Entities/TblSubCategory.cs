using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblSubCategory
{
    public int SubCatId { get; set; }

    public int? CatId { get; set; }

    public string? SubCatName { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public bool? Status { get; set; }
}
