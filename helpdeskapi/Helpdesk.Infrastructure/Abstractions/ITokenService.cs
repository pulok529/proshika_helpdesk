using Helpdesk.Contracts.Tokens;

namespace Helpdesk.Infrastructure.Abstractions;

public interface ITokenService
{
    Task<IReadOnlyList<TokenDto>> ListAsync(int page, int pageSize, int? issueBy, CancellationToken cancellationToken = default);
    Task<TokenDto?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<TokenDetailsDto?> GetDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TokenFileDto>> GetFilesAsync(int tokenId, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string FileName, string ContentType)?> GetFileAsync(int tokenFileId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TokenCommentDto>> GetCommentsAsync(int tokenId, CancellationToken cancellationToken = default);
    Task<TokenEditDto?> GetEditableAsync(int tokenId, CancellationToken cancellationToken = default);
    Task UpdateDetailsAsync(TokenEditDto dto, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(int deleteId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateTokenRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateTokenRequest request, CancellationToken cancellationToken = default);
    Task AddCommentAsync(int tokenId, string comment, string commentBy, int empId, CancellationToken cancellationToken = default);
    Task AddFileAsync(TokenFileUpload upload, CancellationToken cancellationToken = default);
    Task AssignAsync(int tokenId, int solveBy, int issueBy, string? commentBy, CancellationToken cancellationToken = default);
    Task CompleteAsync(int tokenId, int solveBy, string? comment, string? solverName, CancellationToken cancellationToken = default);
    Task SolveAsync(int tokenId, int solveBy, string? status, string? solveByUserName, CancellationToken cancellationToken = default);
    Task TempCloseAsync(int tokenId, int solveBy, string? solveByUserName, CancellationToken cancellationToken = default);
    Task HoldAsync(int tokenId, int solveBy, string? solveByUserName, CancellationToken cancellationToken = default);
    Task ReturnAsync(int tokenId, string comment, CancellationToken cancellationToken = default);
    Task ForwardAsync(int tokenId, string comment, CancellationToken cancellationToken = default);
    Task ReissueAsync(int tokenId, CancellationToken cancellationToken = default);
    Task CloseAsync(int tokenId, CancellationToken cancellationToken = default);
    Task OpenAsync(int tokenId, CancellationToken cancellationToken = default);
    Task CompleteAdminAsync(int tokenId, int solveBy, string? adminCommentStatus, string? solveByUserName, CancellationToken cancellationToken = default);
}
