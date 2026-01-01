using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblPermittedBanch
{
    public int PermittedBranch { get; set; }

    public int? EmpId { get; set; }

    public string? BranchCode { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public bool? Status { get; set; }
}
