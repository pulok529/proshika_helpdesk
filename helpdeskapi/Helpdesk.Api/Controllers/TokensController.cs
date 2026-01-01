using Helpdesk.Contracts.Common;
using Helpdesk.Contracts.Tokens;
using Helpdesk.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Api.Controllers;

[ApiController]
[Route("api/tokens")]
[Authorize]
public class TokensController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] PagedRequest page, CancellationToken cancellationToken)
    {
        var issuerId = int.TryParse(User.FindFirst("sub")?.Value, out var empId) ? empId : (int?)null;
        var items = await _tokenService.ListAsync(page.Page, page.PageSize, issuerId, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetAsync(id, cancellationToken);
        return token is null ? NotFound() : Ok(token);
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> GetEditable(int id, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetEditableAsync(id, cancellationToken);
        return token is null ? NotFound() : Ok(token);
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetDetails(int id, CancellationToken cancellationToken)
    {
        var details = await _tokenService.GetDetailsAsync(id, cancellationToken);
        return details is null ? NotFound() : Ok(details);
    }

    [HttpGet("{id:int}/files")]
    public async Task<IActionResult> GetFiles(int id, CancellationToken cancellationToken)
    {
        var files = await _tokenService.GetFilesAsync(id, cancellationToken);
        return Ok(files);
    }

    [HttpGet("{id:int}/comments")]
    public async Task<IActionResult> GetComments(int id, CancellationToken cancellationToken)
    {
        var comments = await _tokenService.GetCommentsAsync(id, cancellationToken);
        return Ok(comments);
    }

    [HttpGet("{id:int}/files/{fileId:int}")]
    public async Task<IActionResult> DownloadFile(int id, int fileId, CancellationToken cancellationToken)
    {
        var file = await _tokenService.GetFileAsync(fileId, cancellationToken);
        if (file is null) return NotFound();
        var (content, fileName, contentType) = file.Value;
        return File(content, contentType, fileName);
    }

    [HttpDelete("files/{deleteId:int}")]
    public async Task<IActionResult> DeleteFile(int deleteId, CancellationToken cancellationToken)
    {
        await _tokenService.DeleteFileAsync(deleteId, cancellationToken);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTokenRequest request, CancellationToken cancellationToken)
    {
        var issuerId = int.TryParse(User.FindFirst("sub")?.Value, out var empId) ? empId : 0;
        var enriched = request with { IssueBy = issuerId, ProblemDetails = request.ProblemDetails ?? request.Description };
        var id = await _tokenService.CreateAsync(enriched, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}/edit")]
    public async Task<IActionResult> UpdateDetails(int id, [FromBody] TokenEditDto request, CancellationToken cancellationToken)
    {
        if (id != request.TokenId) return BadRequest("Token id mismatch");
        await _tokenService.UpdateDetailsAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTokenRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/comments")]
    public async Task<IActionResult> AddComment(int id, [FromBody] CommentRequest request, CancellationToken cancellationToken)
    {
        var empId = int.TryParse(User.FindFirst("sub")?.Value, out var parsed) ? parsed : 0;
        await _tokenService.AddCommentAsync(id, request.Comment, request.CommentBy ?? User.Identity?.Name ?? "user", empId, cancellationToken);
        return NoContent();
    }

    [RequestSizeLimit(25_000_000)]
    [HttpPost("{id:int}/files")]
    public async Task<IActionResult> UploadFile(int id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0) return BadRequest("File is required");
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms, cancellationToken);
        var empId = int.TryParse(User.FindFirst("sub")?.Value, out var parsed) ? parsed : 0;
        var upload = new TokenFileUpload(id, empId, file.FileName, file.ContentType, ms.ToArray(), null);
        await _tokenService.AddFileAsync(upload, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> Assign(int id, [FromBody] AssignTokenRequest request, CancellationToken cancellationToken)
    {
        var issueBy = int.TryParse(User.FindFirst("sub")?.Value, out var parsed) ? parsed : 0;
        await _tokenService.AssignAsync(id, request.SolveBy, issueBy, request.CommentBy, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id, [FromBody] CompleteTokenRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.CompleteAsync(id, request.SolveBy, request.Comment, request.SolverName, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/solve")]
    public async Task<IActionResult> Solve(int id, [FromBody] TokenStatusRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.SolveAsync(id, request.SolveBy, request.Comment, request.SolveByUserName, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/temp-close")]
    public async Task<IActionResult> TempClose(int id, [FromBody] TokenStatusRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.TempCloseAsync(id, request.SolveBy, request.SolveByUserName, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/hold")]
    public async Task<IActionResult> Hold(int id, [FromBody] TokenStatusRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.HoldAsync(id, request.SolveBy, request.SolveByUserName, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/return")]
    public async Task<IActionResult> Return(int id, [FromBody] TokenReturnRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.ReturnAsync(id, request.Comment, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/forward")]
    public async Task<IActionResult> Forward(int id, [FromBody] TokenForwardRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.ForwardAsync(id, request.Comment, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/reissue")]
    public async Task<IActionResult> Reissue(int id, CancellationToken cancellationToken)
    {
        await _tokenService.ReissueAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id, CancellationToken cancellationToken)
    {
        await _tokenService.CloseAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/open")]
    public async Task<IActionResult> Open(int id, CancellationToken cancellationToken)
    {
        await _tokenService.OpenAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/complete/admin")]
    public async Task<IActionResult> CompleteAdmin(int id, [FromBody] TokenStatusRequest request, CancellationToken cancellationToken)
    {
        await _tokenService.CompleteAdminAsync(id, request.SolveBy, request.Comment, request.SolveByUserName, cancellationToken);
        return NoContent();
    }
}
