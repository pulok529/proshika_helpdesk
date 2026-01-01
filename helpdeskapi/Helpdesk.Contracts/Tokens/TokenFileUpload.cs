namespace Helpdesk.Contracts.Tokens;

public record TokenFileUpload(int TokenId, int IssueBy, string FileName, string ContentType, byte[] Content, int? DeleteId = null);
