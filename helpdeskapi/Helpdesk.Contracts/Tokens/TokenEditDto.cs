namespace Helpdesk.Contracts.Tokens;

public record TokenEditDto(
    int TokenId,
    int? CatId,
    int? SubCatId,
    int? TrdCatId,
    int? GroupCode,
    int? MemberCode,
    int? BranchCode,
    string? ProblemDetails);
