using Helpdesk.Contracts.Support;
using Helpdesk.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Api.Controllers;

[ApiController]
[Route("api/support-users")]
[Authorize]
public class SupportUsersController : ControllerBase
{
    private readonly ISupportUserService _service;

    public SupportUsersController(ISupportUserService service)
    {
        _service = service;
    }

    [HttpGet("unassigned")]
    public async Task<IActionResult> GetUnassigned(CancellationToken cancellationToken)
    {
        var items = await _service.GetUnassignedAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet]
    public async Task<IActionResult> GetAssigned(CancellationToken cancellationToken)
    {
        var items = await _service.GetAssignedAsync(cancellationToken);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Assign([FromBody] AssignSupportUserRequest request, CancellationToken cancellationToken)
    {
        var entryBy = int.TryParse(User.FindFirst("sub")?.Value, out var empId) ? empId : 0;
        if (entryBy == 0) return Unauthorized("User id missing in token.");

        await _service.AssignAsync(request.EmpId, entryBy, cancellationToken);
        return NoContent();
    }

    [HttpPost("{permittedId:int}/activate")]
    public async Task<IActionResult> Activate(int permittedId, CancellationToken cancellationToken)
    {
        var entryBy = int.TryParse(User.FindFirst("sub")?.Value, out var empId) ? empId : 0;
        if (entryBy == 0) return Unauthorized("User id missing in token.");

        await _service.SetStatusAsync(permittedId, entryBy, active: true, cancellationToken);
        return NoContent();
    }

    [HttpPost("{permittedId:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int permittedId, CancellationToken cancellationToken)
    {
        var entryBy = int.TryParse(User.FindFirst("sub")?.Value, out var empId) ? empId : 0;
        if (entryBy == 0) return Unauthorized("User id missing in token.");

        await _service.SetStatusAsync(permittedId, entryBy, active: false, cancellationToken);
        return NoContent();
    }
}
