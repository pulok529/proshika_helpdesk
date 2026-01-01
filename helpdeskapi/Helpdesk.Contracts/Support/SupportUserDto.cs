namespace Helpdesk.Contracts.Support;

public record SupportUserDto(
    int PermittedId,
    int EmpId,
    string? Name,
    string? Designation,
    string? RoleName,
    string? EntryUserName,
    string? Status,
    DateTime? EntryDate);
