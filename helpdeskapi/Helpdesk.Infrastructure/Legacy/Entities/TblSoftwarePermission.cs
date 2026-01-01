using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblSoftwarePermission
{
    public int SupportSoftwareId { get; set; }

    public int? SoftCatId { get; set; }

    public int? SupportPerson { get; set; }

    public bool? Status { get; set; }
}
