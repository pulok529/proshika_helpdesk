namespace Helpdesk.Contracts.Tokens;

public record TokenDetailsDto(
    int TokenId,
    int? SolveBy,
    int? IssueBy,
    int? GroupCode,
    int? MemberCode,
    int? BranchCode,
    string? IssueUserName,
    string? IssueUserBranch,
    string? TokenSerialId,
    string? CatName,
    string? SubCatName,
    string? TrdCatName,
    string? SolveByUserName,
    string? Priority,
    string? ProblemDetails,
    DateTime IssueDate);
