namespace Helpdesk.Contracts.Tokens;

public record TokenFileDto(int TokenFileId, int? IssueBy, string? IssueUserName, DateTime? IssueDate, string FileName, string? ContentType);
