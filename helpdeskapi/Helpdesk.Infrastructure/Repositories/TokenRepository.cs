using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Repositories;
using Helpdesk.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly HelpdeskDbContext _dbContext;

    public TokenRepository(HelpdeskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateAsync(Token token, CancellationToken cancellationToken = default)
    {
        _dbContext.Tokens.Add(token);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return token.Id;
    }

    public async Task<Token?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tokens.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Token>> ListAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tokens
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Token token, CancellationToken cancellationToken = default)
    {
        _dbContext.Tokens.Update(token);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
