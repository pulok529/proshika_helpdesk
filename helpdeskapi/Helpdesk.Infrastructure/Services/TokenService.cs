using Helpdesk.Contracts.Tokens;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Repositories;
using Helpdesk.Infrastructure.Abstractions;

namespace Helpdesk.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly ITokenRepository _repository;
    private readonly TokenSpService _spService;

    public TokenService(ITokenRepository repository, TokenSpService spService)
    {
        _repository = repository;
        _spService = spService;
    }

    public Task<TokenDetailsDto?> GetDetailsAsync(int id, CancellationToken cancellationToken = default) =>
        _spService.GetDetailsAsync(id, cancellationToken);

    public Task<IReadOnlyList<TokenFileDto>> GetFilesAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _spService.GetFilesAsync(tokenId, cancellationToken);

    public Task<(byte[] Content, string FileName, string ContentType)?> GetFileAsync(int tokenFileId, CancellationToken cancellationToken = default) =>
        _spService.GetFileContentAsync(tokenFileId, cancellationToken);

    public Task<IReadOnlyList<TokenCommentDto>> GetCommentsAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _spService.GetCommentsAsync(tokenId, cancellationToken);

    public Task<TokenEditDto?> GetEditableAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _spService.GetEditableAsync(tokenId, cancellationToken);

    public Task UpdateDetailsAsync(TokenEditDto dto, CancellationToken cancellationToken = default) =>
        _spService.UpdateDetailsAsync(dto, cancellationToken);

    public Task DeleteFileAsync(int deleteId, CancellationToken cancellationToken = default) =>
        _spService.DeleteFileAsync(deleteId, cancellationToken);

    public async Task<int> CreateAsync(CreateTokenRequest request, CancellationToken cancellationToken = default)
    {
        // Use legacy stored proc for creation so behavior matches original system
        var issueBy = request.IssueBy ?? 0;
        return await _spService.CreateAsync(request, issueBy, cancellationToken);
    }

    public async Task<TokenDto?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _spService.GetAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<TokenDto>> ListAsync(int page, int pageSize, int? issueBy, CancellationToken cancellationToken = default)
    {
        // Legacy list by issuer is closer to current UI expectations; default to provided issuer or all via repository.
        if (issueBy.HasValue)
        {
            return await _spService.ListByIssuerAsync(issueBy.Value, cancellationToken);
        }
        var items = await _repository.ListAsync(page, pageSize, cancellationToken);
        return items.Select(ToDto).ToList();
    }

    public async Task UpdateAsync(int id, UpdateTokenRequest request, CancellationToken cancellationToken = default)
    {
        var token = await _repository.GetByIdAsync(id, cancellationToken);
        if (token is null)
        {
            throw new KeyNotFoundException($"Token {id} not found");
        }

        token.Title = request.Title;
        token.Description = request.Description;
        token.Status = request.Status;
        token.AssignedTo = request.AssignedTo;

        await _repository.UpdateAsync(token, cancellationToken);
    }

    public Task AddCommentAsync(int tokenId, string comment, string commentBy, int empId, CancellationToken cancellationToken = default) =>
        _spService.AddCommentAsync(tokenId, comment, commentBy, empId, cancellationToken);

    public Task AddFileAsync(TokenFileUpload upload, CancellationToken cancellationToken = default) =>
        _spService.AddFileAsync(upload, cancellationToken);

    public Task AssignAsync(int tokenId, int solveBy, int issueBy, string? commentBy, CancellationToken cancellationToken = default) =>
        _spService.AssignAsync(tokenId, solveBy, issueBy, commentBy, cancellationToken);

    public Task CompleteAsync(int tokenId, int solveBy, string? comment, string? solverName, CancellationToken cancellationToken = default) =>
        _spService.CompleteAsync(tokenId, solveBy, comment, solverName, cancellationToken);

    public Task SolveAsync(int tokenId, int solveBy, string? status, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _spService.SolveAsync(tokenId, solveBy, status, solveByUserName, cancellationToken);

    public Task TempCloseAsync(int tokenId, int solveBy, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _spService.TempCloseAsync(tokenId, solveBy, solveByUserName, cancellationToken);

    public Task HoldAsync(int tokenId, int solveBy, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _spService.HoldAsync(tokenId, solveBy, solveByUserName, cancellationToken);

    public Task ReturnAsync(int tokenId, string comment, CancellationToken cancellationToken = default) =>
        _spService.ReturnAsync(tokenId, comment, cancellationToken);

    public Task ForwardAsync(int tokenId, string comment, CancellationToken cancellationToken = default) =>
        _spService.ForwardAsync(tokenId, comment, cancellationToken);

    public Task ReissueAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _spService.ReissueAsync(tokenId, cancellationToken);

    public Task CloseAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _spService.CloseAsync(tokenId, cancellationToken);

    public Task OpenAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _spService.OpenAsync(tokenId, cancellationToken);

    public Task CompleteAdminAsync(int tokenId, int solveBy, string? adminCommentStatus, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _spService.CompleteAdminAsync(tokenId, solveBy, adminCommentStatus, solveByUserName, cancellationToken);

    private static TokenDto ToDto(Token token) =>
        new(token.Id, token.Title, token.Description, token.Status, token.CreatedAt, token.AssignedTo);
}
