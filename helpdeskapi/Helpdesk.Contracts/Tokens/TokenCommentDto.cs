namespace Helpdesk.Contracts.Tokens;

public record TokenCommentDto(int CommentId, int TokenId, int CommentedByEmpId, string? CommentBy, string? Comment, DateTime CommentedAt, string? CommentStatus);
