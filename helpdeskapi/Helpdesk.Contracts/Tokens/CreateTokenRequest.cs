namespace Helpdesk.Contracts.Tokens;

public record CreateTokenRequest(
    string Title,
    string? Description,
    int? AssignedTo,
    string? Priority,
    int? CatId,
    int? SubCatId,
    int? TrdCatId,
    int? BranchCode,
    int? GroupCode,
    int? MemberCode,
    int? SolveBy,
    int? IssueBy,
    string? ProblemDetails);
