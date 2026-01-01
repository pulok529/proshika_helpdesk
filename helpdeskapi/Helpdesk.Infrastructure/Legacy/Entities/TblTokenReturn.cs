using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblTokenReturn
{
    public int TokenReturnId { get; set; }

    public int? TokenId { get; set; }

    public string? Remarks { get; set; }
}
