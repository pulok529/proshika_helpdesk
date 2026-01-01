using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleActivity { get; set; }
}
