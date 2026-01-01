using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblComment
{
    public int CommentId { get; set; }

    public string? Comment { get; set; }

    public int? TokenId { get; set; }

    public string? CommentBy { get; set; }

    public DateTime? CommentDateTime { get; set; }

    public string? CommentStatus { get; set; }

    public int? CmntEmpId { get; set; }
}
