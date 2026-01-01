using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblModalChecker
{
    public int CheckId { get; set; }

    public int? EmpId { get; set; }

    public string? ModalView { get; set; }
}
