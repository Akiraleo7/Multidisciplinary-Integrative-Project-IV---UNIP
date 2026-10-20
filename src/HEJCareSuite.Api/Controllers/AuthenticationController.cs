using HEJCareSuite.Api.Contracts.Authentication;
using HEJCareSuite.Application.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        if (user is null)
            return Unauthorized(new { message = "Invalid email or password." });

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme)));

        return Ok(new { user.Email, user.Role });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }
}
