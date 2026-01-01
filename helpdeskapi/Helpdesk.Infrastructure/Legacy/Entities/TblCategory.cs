using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblCategory
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public bool? Status { get; set; }
}
