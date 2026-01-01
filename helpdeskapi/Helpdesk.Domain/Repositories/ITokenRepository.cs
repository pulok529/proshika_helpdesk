using Helpdesk.Domain.Entities;

namespace Helpdesk.Domain.Repositories;

public interface ITokenRepository
{
    Task<IReadOnlyList<Token>> ListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Token?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Token token, CancellationToken cancellationToken = default);
    Task UpdateAsync(Token token, CancellationToken cancellationToken = default);
}
