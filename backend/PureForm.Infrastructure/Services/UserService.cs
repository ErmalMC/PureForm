using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email);
            var user = users.FirstOrDefault();
            return user == null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
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
            return MapToDto(created);
        }

        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            // Update basic info
            if (dto.FirstName != null) user.FirstName = dto.FirstName;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.Weight.HasValue) user.Weight = dto.Weight.Value;
            if (dto.Height.HasValue) user.Height = dto.Height.Value;
            if (dto.FitnessGoal != null) user.FitnessGoal = dto.FitnessGoal;

            // ADD THESE LINES - Update nutrition goals
            if (dto.DailyCalorieGoal.HasValue) user.DailyCalorieGoal = dto.DailyCalorieGoal.Value;
            if (dto.DailyProteinGoal.HasValue) user.DailyProteinGoal = dto.DailyProteinGoal.Value;
            if (dto.DailyCarbsGoal.HasValue) user.DailyCarbsGoal = dto.DailyCarbsGoal.Value;
            if (dto.DailyFatsGoal.HasValue) user.DailyFatsGoal = dto.DailyFatsGoal.Value;

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        // UPDATED - Include nutrition goals in the DTO mapping
        private static UserDto MapToDto(User user) => new()
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
            IsPremium = user.IsPremium,
            // ADD THESE LINES
            DailyCalorieGoal = user.DailyCalorieGoal,
            DailyProteinGoal = user.DailyProteinGoal,
            DailyCarbsGoal = user.DailyCarbsGoal,
            DailyFatsGoal = user.DailyFatsGoal
        };
    }
}