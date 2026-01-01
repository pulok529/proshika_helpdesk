using Helpdesk.Contracts.Auth;
using Helpdesk.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login/internal")]
    public async Task<ActionResult<LoginResponse>> LoginInternal([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginInternalAsync(request.Username, cancellationToken);
        return result is null ? Unauthorized() : Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login/vendor")]
    public async Task<ActionResult<LoginResponse>> LoginVendor([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginVendorAsync(request.Username, request.Password, cancellationToken);
        return result is null ? Unauthorized() : Ok(result);
    }
}
