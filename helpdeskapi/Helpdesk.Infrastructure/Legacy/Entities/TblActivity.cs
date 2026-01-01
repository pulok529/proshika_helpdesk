using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblActivity
{
    public int ActivityId { get; set; }

    public int? AcitivityBy { get; set; }

    public DateTime? ActivityDateTime { get; set; }

    public string? ActivitySection { get; set; }
}
