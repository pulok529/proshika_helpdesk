using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblTrdCategory
{
    public int TrdCatId { get; set; }

    public int? CatId { get; set; }

    public int? SubCatId { get; set; }

    public string? TrdCatName { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public bool? Status { get; set; }
}
