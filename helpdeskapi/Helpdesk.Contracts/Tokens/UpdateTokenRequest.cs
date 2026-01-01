namespace Helpdesk.Contracts.Tokens;

public record UpdateTokenRequest(
    string Title,
    string? Description,
    string Status,
    int? AssignedTo,
    int? CatId,
    int? SubCatId,
    int? TrdCatId,
    int? BranchCode,
    int? GroupCode,
    int? MemberCode,
    int? SolveBy,
    string? ProblemDetails);
