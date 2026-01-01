namespace Helpdesk.Contracts.Tokens;

public record TokenDto(int Id, string? Title, string? Description, string? Status, DateTime CreatedAt, int? AssignedTo);
