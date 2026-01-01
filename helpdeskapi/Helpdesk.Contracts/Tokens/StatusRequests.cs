namespace Helpdesk.Contracts.Tokens;

public record TokenStatusRequest(int SolveBy, string? SolveByUserName, string? Comment);
public record TokenReturnRequest(string Comment);
public record TokenForwardRequest(string Comment);
