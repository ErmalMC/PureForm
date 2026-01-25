using PureForm.Application.DTOs;

namespace PureForm.Application.Interfaces;

public interface IWorkoutPlanService
{
    Task<WorkoutPlanDto?> GetByIdAsync(int id);
    Task<IEnumerable<WorkoutPlanDto>> GetByUserIdAsync(int userId);
    Task<WorkoutPlanDto> CreateAsync(int userId, CreateWorkoutPlanDto dto);
    Task<WorkoutPlanDto?> UpdateAsync(int id, CreateWorkoutPlanDto dto);
    Task<bool> DeleteAsync(int id);
    Task<WorkoutPlanDto?> GeneratePersonalizedPlanAsync(int userId, string? difficultyLevel = null);
}