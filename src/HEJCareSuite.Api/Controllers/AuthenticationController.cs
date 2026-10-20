using HEJCareSuite.Api.Contracts.Authentication;
using HEJCareSuite.Application.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HEJCareSuite.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthenticationController(AuthenticateUser authenticateUser)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await authenticateUser.ExecuteAsync(
            new AuthenticateUserCommand(request.Email, request.Password),
            cancellationToken);

        return user is null
            ? Unauthorized(new { message = "Invalid email or password." })
            : Ok(new { user.Email, user.Role });
    }
}
