using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Application.Modules.Auth.Dtos;
using Staffinity.Personal.Application.Modules.Auth.UseCases;

namespace Staffinity.Personal.Api.Modules.Auth.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ILoginUseCase _loginUseCase;

    public AuthController(ILoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request is null)
        {
            return BadRequest(new { error = "request_required" });
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { error = "email_required" });
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "password_required" });
        }

        try
        {
            var response = await _loginUseCase.ExecuteAsync(request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = "validation_error", details = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { error = "invalid_credentials" });
        }
    }
}
