using Helpdesk.Contracts.Support;

namespace Helpdesk.Infrastructure.Abstractions;

public interface ISupportUserService
{
    Task<IReadOnlyList<SupportUserCandidateDto>> GetUnassignedAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupportUserDto>> GetAssignedAsync(CancellationToken cancellationToken = default);
    Task AssignAsync(int empId, int entryBy, CancellationToken cancellationToken = default);
    Task SetStatusAsync(int permittedId, int entryBy, bool active, CancellationToken cancellationToken = default);
}
