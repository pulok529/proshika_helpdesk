namespace Helpdesk.Contracts.Auth;

public record LoginResponse(string AccessToken, DateTime ExpiresAtUtc, string? RefreshToken = null);
