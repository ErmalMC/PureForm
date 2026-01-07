using Microsoft.AspNetCore.Mvc;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;

namespace PureForm.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        // Check if user already exists
        if (await _authService.UserExistsAsync(dto.Email))
        {
            return BadRequest(new { message = "User with this email already exists" });
        }

        var response = await _authService.RegisterAsync(dto);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var response = await _authService.LoginAsync(dto);

        if (response == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        return Ok(response);
    }

    [HttpGet("check-email")]
    public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email)
    {
        var exists = await _authService.UserExistsAsync(email);
        return Ok(new { exists });
    }
}