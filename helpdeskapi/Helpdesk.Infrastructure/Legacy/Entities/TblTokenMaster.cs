using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblTokenMaster
{
    public int TokenId { get; set; }

    public DateTime? IssueDate { get; set; }

    public int? IssueBy { get; set; }

    public string? Priority { get; set; }

    public int? CatId { get; set; }

    public int? SubCatId { get; set; }

    public int? TrdCatId { get; set; }

    public int? BranchCode { get; set; }

    public int? GroupCode { get; set; }

    public int? MemberCode { get; set; }

    public string? ProblemDetails { get; set; }

    public string? Status { get; set; }

    public string? TokenSerialId { get; set; }

    public string? ReturnToken { get; set; }

    public int? TokenClosedBy { get; set; }

    public int? SolveBy { get; set; }

    public DateTime? AssignedDateTime { get; set; }

    public DateTime? TokenReceiveDateTime { get; set; }

    public DateTime? SolveDateTime { get; set; }
}
