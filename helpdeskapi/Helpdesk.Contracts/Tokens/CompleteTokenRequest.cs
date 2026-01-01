namespace Helpdesk.Contracts.Tokens;

public record CompleteTokenRequest(int SolveBy, string? Comment, string? SolverName);
