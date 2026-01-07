using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Repositories;

namespace PureForm.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IRepository<User> userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var users = await _userRepository.FindAsync(u => u.Email == dto.Email);
        var user = users.FirstOrDefault();

        if (user == null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        var token = _jwtService.GenerateToken(user);
        var expiryMinutes = 1440; // Should come from config

        return new AuthResponseDto
        {
            Token = token,
            User = MapToUserDto(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Weight = dto.Weight,
            Height = dto.Height,
            Gender = dto.Gender,
            FitnessGoal = dto.FitnessGoal,
            IsPremium = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.AddAsync(user);
        var token = _jwtService.GenerateToken(created);
        var expiryMinutes = 1440;

        return new AuthResponseDto
        {
            Token = token,
            User = MapToUserDto(created),
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
        };
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        var users = await _userRepository.FindAsync(u => u.Email == email);
        return users.Any();
    }

    private static UserDto MapToUserDto(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        DateOfBirth = user.DateOfBirth,
        Weight = user.Weight,
        Height = user.Height,
        Gender = user.Gender,
        FitnessGoal = user.FitnessGoal,
        IsPremium = user.IsPremium
    };
}