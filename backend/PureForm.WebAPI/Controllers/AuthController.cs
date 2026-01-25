using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Infrastructure.Repositories;
using System.Security.Claims;

namespace PureForm.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IRepository<PureForm.Domain.Entities.User> _userRepository;

    public AuthController(IAuthService authService, IRepository<PureForm.Domain.Entities.User> userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
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

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            id = user.Id,
            email = user.Email,
            firstName = user.FirstName,
            lastName = user.LastName,
            weight = user.Weight,
            height = user.Height,
            fitnessGoal = user.FitnessGoal,
            isPremium = user.IsPremium
        });
    }
}