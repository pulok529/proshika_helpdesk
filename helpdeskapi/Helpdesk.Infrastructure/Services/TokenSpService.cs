using System.Data;
using Microsoft.Data.SqlClient;
using Helpdesk.Contracts.Tokens;
using Helpdesk.Infrastructure.Legacy;

namespace Helpdesk.Infrastructure.Services;

public class TokenSpService
{
    private readonly LegacyDataAccess _dataAccess;

    public TokenSpService(LegacyDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<int> CreateAsync(CreateTokenRequest request, int issueBy, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@IssueBy", issueBy),
            new("@IssueDate", DateTime.Now),
            new("@Priority", request.Priority ?? string.Empty),
            new("@CatId", request.CatId ?? (object)DBNull.Value),
            new("@SubCatId", request.SubCatId ?? (object)DBNull.Value),
            new("@ProblemDetails", request.ProblemDetails ?? string.Empty),
            new("@SolveBy", request.SolveBy ?? (object)DBNull.Value),
            new("@BranchCode", request.BranchCode ?? (object)DBNull.Value),
            new("@GroupCode", request.GroupCode ?? (object)DBNull.Value),
            new("@MemberCode", request.MemberCode ?? (object)DBNull.Value),
            new("@TrdCatId", request.TrdCatId ?? (object)DBNull.Value)
        };
        return await _dataAccess.ExecuteScalarIntAsync("sp_Save_Token", parameters, cancellationToken);
    }

    public async Task<TokenDto?> GetAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter> { new("@TokenId", tokenId) };
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_Token_Details", parameters, cancellationToken);
        if (dt.Rows.Count == 0) return null;
        var row = dt.Rows[0];
        return new TokenDto(
            row.Field<int>("TokenId"),
            row.Field<string?>("CatName"),
            row.Field<string?>("ProblemDetails"),
            row.Field<string?>("Status"),
            row.Field<DateTime?>("IssueDate") ?? DateTime.MinValue,
            row.Field<int?>("SolveBy"));
    }

    public async Task<IReadOnlyList<TokenDto>> ListByIssuerAsync(int issueBy, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter> { new("@IssueBy", issueBy) };
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_Token_List_EntryBy", parameters, cancellationToken);
        var list = new List<TokenDto>();
        foreach (DataRow row in dt.Rows)
        {
            list.Add(new TokenDto(
                row.Field<int>("TokenId"),
                row.Field<string?>("CatName"),
                row.Field<string?>("ProblemDetails"),
                row.Field<string?>("Status"),
                row.Field<DateTime?>("IssueDate") ?? DateTime.MinValue,
                row.Field<int?>("SolveBy")));
        }
        return list;
    }

    public async Task<TokenDetailsDto?> GetDetailsAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_Token_Details", new[] { new SqlParameter("@TokenId", tokenId) }, cancellationToken);
        if (dt.Rows.Count == 0) return null;
        var row = dt.Rows[0];
        return new TokenDetailsDto(
            row.Field<int>("TokenId"),
            row.Field<int?>("SolveBy"),
            row.Field<int?>("IssueBy"),
            row.Field<int?>("GroupCode"),
            row.Field<int?>("MemberCode"),
            row.Field<int?>("BranchCode"),
            row.Field<string?>("IssueUserName"),
            row.Field<string?>("IssueUserBranch"),
            row.Field<string?>("TokenSerialId"),
            row.Field<string?>("CatName"),
            row.Field<string?>("SubCatName"),
            row.Field<string?>("TrdCatName"),
            row.Field<string?>("SolveByUserName"),
            row.Field<string?>("Priority"),
            row.Field<string?>("ProblemDetails"),
            row.Field<DateTime?>("IssueDate") ?? DateTime.MinValue);
    }

    public async Task<IReadOnlyList<TokenFileDto>> GetFilesAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_Token_Files", new[] { new SqlParameter("@TokenId", tokenId) }, cancellationToken);
        var files = new List<TokenFileDto>();
        foreach (DataRow row in dt.Rows)
        {
            files.Add(new TokenFileDto(
                row.Field<int>("TokenFileId"),
                row.Field<int?>("IssueBy"),
                row.Field<string?>("IssueUserName"),
                row.Field<DateTime?>("IssueDate"),
                row.Field<string?>("TokenFileName") ?? string.Empty,
                row.Field<string?>("TokenFileType")));
        }

        return files;
    }

    public async Task<(byte[] Content, string FileName, string ContentType)?> GetFileContentAsync(int tokenFileId, CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_Token_Image_By_IMGId", new[] { new SqlParameter("@TokenFileId", tokenFileId) }, cancellationToken);
        if (dt.Rows.Count == 0) return null;
        var row = dt.Rows[0];
        return (
            (byte[])row["TokenFile"],
            row.Field<string?>("TokenFileName") ?? $"file-{tokenFileId}",
            row.Field<string?>("TokenFileType") ?? "application/octet-stream");
    }

    public async Task<IReadOnlyList<TokenCommentDto>> GetCommentsAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_IT_PERSON_COMMENT_BY_TID", new[] { new SqlParameter("@TokenId", tokenId) }, cancellationToken);
        var comments = new List<TokenCommentDto>();
        foreach (DataRow row in dt.Rows)
        {
            comments.Add(new TokenCommentDto(
                row.Field<int>("CommentId"),
                row.Field<int>("TokenId"),
                row.Field<int>("CmntEmpId"),
                row.Field<string?>("CommentBy"),
                row.Field<string?>("Comment"),
                row.Field<DateTime>("CommentDateTime"),
                row.Field<string?>("CommentStatus")));
        }

        return comments;
    }

    public async Task<TokenEditDto?> GetEditableAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_Token_Edit", new[] { new SqlParameter("@TokenId", tokenId) }, cancellationToken);
        if (dt.Rows.Count == 0) return null;
        var row = dt.Rows[0];
        return new TokenEditDto(
            row.Field<int>("TokenId"),
            row.Field<int?>("CatId"),
            row.Field<int?>("SubCatId"),
            row.Field<int?>("TrdCatId"),
            row.Field<int?>("GroupCode"),
            row.Field<int?>("MemberCode"),
            row.Field<int?>("BranchCode"),
            row.Field<string?>("ProblemDetails"));
    }

    public Task UpdateDetailsAsync(TokenEditDto dto, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Update_Token", new[]
        {
            new SqlParameter("@CatId", dto.CatId ?? (object)DBNull.Value),
            new SqlParameter("@SubCatId", dto.SubCatId ?? (object)DBNull.Value),
            new SqlParameter("@ProblemDetails", dto.ProblemDetails ?? string.Empty),
            new SqlParameter("@BranchCode", dto.BranchCode ?? (object)DBNull.Value),
            new SqlParameter("@GroupCode", dto.GroupCode ?? (object)DBNull.Value),
            new SqlParameter("@MemberCode", dto.MemberCode ?? (object)DBNull.Value),
            new SqlParameter("@TrdCatId", dto.TrdCatId ?? (object)DBNull.Value),
            new SqlParameter("@TokenId", dto.TokenId)
        }, cancellationToken);

    public Task DeleteFileAsync(int deleteId, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Delete_Image", new[] { new SqlParameter("@DeleteId", deleteId) }, cancellationToken);

    public async Task AddCommentAsync(int tokenId, string comment, string commentBy, int empId, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@CmntEmpId", empId),
            new("@Comment", comment),
            new("@CommentBy", commentBy),
            new("@CommentDateTime", DateTime.Now),
            new("@TokenId", tokenId)
        };
        await _dataAccess.ExecuteScalarIntAsync("sp_Save_Comment_User", parameters, cancellationToken);
    }

    public async Task AddFileAsync(TokenFileUpload upload, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@TokenId", upload.TokenId),
            new("@IssueBy", upload.IssueBy),
            new("@IssueDate", DateTime.Now),
            new("@TokenFile", upload.Content),
            new("@TokenFileName", upload.FileName),
            new("@TokenFileType", upload.ContentType),
            new("@DeleteId", upload.DeleteId ?? new Random().Next())
        };
        await _dataAccess.ExecuteScalarIntAsync("sp_Save_File", parameters, cancellationToken);
    }

    public async Task AssignAsync(int tokenId, int solveBy, int issueBy, string? commentBy, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@TokenId", tokenId),
            new("@SolveBy", solveBy),
            new("@IssueBy", issueBy),
            new("@SolveDateTime", DateTime.Now),
            new("@CommentBy", commentBy ?? string.Empty)
        };
        await _dataAccess.ExecuteScalarIntAsync("sp_Save_ReAssigned", parameters, cancellationToken);
    }

    public async Task CompleteAsync(int tokenId, int solveBy, string? comment, string? solverName, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@Comment", comment ?? string.Empty),
            new("@SolveByUserName", solverName ?? string.Empty),
            new("@TokenId", tokenId),
            new("@SolveDate", DateTime.Now),
            new("@IssueBy", solveBy)
        };
        await _dataAccess.ExecuteScalarIntAsync("sp_Complete_Token_By_IT_Person", parameters, cancellationToken);
    }

    public Task SolveAsync(int tokenId, int solveBy, string? status, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Solve_Token", new[]
        {
            new SqlParameter("@TokenId", tokenId),
            new SqlParameter("@SolveBy", solveBy),
            new SqlParameter("@SolveDateTime", DateTime.Now),
            new SqlParameter("@Status", status ?? "Solve"),
            new SqlParameter("@SolveByUserName", solveByUserName ?? string.Empty)
        }, cancellationToken);

    public Task TempCloseAsync(int tokenId, int solveBy, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_TempCose_Token", new[]
        {
            new SqlParameter("@TokenId", tokenId),
            new SqlParameter("@SolveBy", solveBy),
            new SqlParameter("@SolveDateTime", DateTime.Now),
            new SqlParameter("@SolveByUserName", solveByUserName ?? string.Empty)
        }, cancellationToken);

    public Task HoldAsync(int tokenId, int solveBy, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Hold_Token", new[]
        {
            new SqlParameter("@TokenId", tokenId),
            new SqlParameter("@SolveBy", solveBy),
            new SqlParameter("@SolveDateTime", DateTime.Now),
            new SqlParameter("@SolveByUserName", solveByUserName ?? string.Empty)
        }, cancellationToken);

    public Task ReturnAsync(int tokenId, string comment, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Return_Token", new[]
        {
            new SqlParameter("@Comment", comment),
            new SqlParameter("@TokenId", tokenId),
            new SqlParameter("@ReturnDateTime", DateTime.Now)
        }, cancellationToken);

    public Task ForwardAsync(int tokenId, string comment, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Forward_Token", new[]
        {
            new SqlParameter("@Comment", comment),
            new SqlParameter("@TokenId", tokenId),
            new SqlParameter("@ReturnDateTime", DateTime.Now)
        }, cancellationToken);

    public Task ReissueAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Update_Token_Re_Issue", new[]
        {
            new SqlParameter("@TokenId", tokenId)
        }, cancellationToken);

    public Task CloseAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Update_Token_As_Closed", new[]
        {
            new SqlParameter("@TokenId", tokenId)
        }, cancellationToken);

    public Task OpenAsync(int tokenId, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Update_Token_As_OPENORNOT", new[]
        {
            new SqlParameter("@TokenId", tokenId)
        }, cancellationToken);

    public Task CompleteAdminAsync(int tokenId, int solveBy, string? adminCommentStatus, string? solveByUserName, CancellationToken cancellationToken = default) =>
        _dataAccess.ExecuteScalarIntAsync("sp_Complete_Token_By_Admin", new[]
        {
            new SqlParameter("@AdminCommentStatus", adminCommentStatus ?? string.Empty),
            new SqlParameter("@SolveByUserName", solveByUserName ?? string.Empty),
            new SqlParameter("@TokenId", tokenId),
            new SqlParameter("@SolveDate", DateTime.Now),
            new SqlParameter("@SolveBy", solveBy)
        }, cancellationToken);
}
