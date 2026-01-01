using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblTokenFile
{
    public int TokenFileId { get; set; }

    public byte[]? TokenFile { get; set; }

    public string? TokenFileName { get; set; }

    public string? TokenFileType { get; set; }

    public int? TokenId { get; set; }

    public int? DeleteId { get; set; }

    public DateTime? IssueDate { get; set; }

    public int? IssueBy { get; set; }
}
