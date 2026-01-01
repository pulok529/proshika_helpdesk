using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblPermittedUser
{
    public int PermittedId { get; set; }

    public int? EmpId { get; set; }

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? WorkingAs { get; set; }
}
