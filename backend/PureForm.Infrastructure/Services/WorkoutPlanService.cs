using Microsoft.EntityFrameworkCore;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Application.Utilities;
using PureForm.Domain.Entities;
using PureForm.Domain.Enums;
using PureForm.Infrastructure.Data;
using PureForm.Infrastructure.Repositories;
namespace PureForm.Infrastructure.Services;
public class WorkoutPlanService : IWorkoutPlanService
{
    private readonly IRepository<WorkoutPlan> _workoutPlanRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Exercise> _exerciseRepository;
    private readonly ApplicationDbContext _context;
    public WorkoutPlanService(
        IRepository<WorkoutPlan> workoutPlanRepository,
        IRepository<User> userRepository,
        IRepository<Exercise> exerciseRepository,
        ApplicationDbContext context)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _userRepository = userRepository;
        _exerciseRepository = exerciseRepository;
        _context = context;
    }
    public async Task<WorkoutPlanDto?> GetByIdAsync(int id)
    {
        var plan = await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .FirstOrDefaultAsync(wp => wp.Id == id);
        return plan == null ? null : MapToDto(plan);
    }
    public async Task<IEnumerable<WorkoutPlanDto>> GetByUserIdAsync(int userId)
    {
        var plans = await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .Where(wp => wp.UserId == userId)
            .ToListAsync();
        return plans.Select(MapToDto);
    }
    public async Task<WorkoutPlanDto> CreateAsync(int userId, CreateWorkoutPlanDto dto)
    {
        var difficultyEnum = EnumConverter.ParseDifficultyLevel(dto.DifficultyLevel) ?? DifficultyLevel.Beginner;
        var plan = new WorkoutPlan
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
            DifficultyLevel = difficultyEnum,
            DurationWeeks = dto.DurationWeeks,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _workoutPlanRepository.AddAsync(plan);
        return MapToDto(created);
    }
    public async Task<WorkoutPlanDto?> UpdateAsync(int id, CreateWorkoutPlanDto dto)
    {
        var plan = await _workoutPlanRepository.GetByIdAsync(id);
        if (plan == null) return null;
        plan.Name = dto.Name;
        plan.Description = dto.Description;
        var difficultyEnum = EnumConverter.ParseDifficultyLevel(dto.DifficultyLevel);
        if (difficultyEnum.HasValue)
            plan.DifficultyLevel = difficultyEnum.Value;
        plan.DurationWeeks = dto.DurationWeeks;
        plan.UpdatedAt = DateTime.UtcNow;
        await _workoutPlanRepository.UpdateAsync(plan);
        return MapToDto(plan);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var plan = await _workoutPlanRepository.GetByIdAsync(id);
        if (plan == null) return false;
        await _workoutPlanRepository.DeleteAsync(plan);
        return true;
    }
    public async Task<WorkoutPlanDto?> GeneratePersonalizedPlanAsync(int userId, string? difficultyLevel = null)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return null;
        var difficultyEnum = EnumConverter.ParseDifficultyLevel(difficultyLevel) ?? DifficultyLevel.Intermediate;
        var difficultyString = difficultyEnum.ToString();
        var planName = $"{difficultyString} {user.FitnessGoal} Plan";
        var description = $"Custom generated {difficultyString.ToLower()} workout plan customized for {user.FirstName}'s {user.FitnessGoal.ToString().ToLower()} goals";
        var plan = new WorkoutPlan
        {
            UserId = userId,
            Name = planName,
            Description = description,
            DifficultyLevel = difficultyEnum,
            DurationWeeks = difficultyEnum == DifficultyLevel.Beginner ? 4 : difficultyEnum == DifficultyLevel.Intermediate ? 8 : 12,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _workoutPlanRepository.AddAsync(plan);
        var exercises = GenerateExercisesForGoalAndDifficulty(user.FitnessGoal, difficultyString, created.Id);
        foreach (var exercise in exercises)
        {
            await _exerciseRepository.AddAsync(exercise);
        }
        var planWithExercises = await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .FirstOrDefaultAsync(wp => wp.Id == created.Id);
        return planWithExercises == null ? null : MapToDto(planWithExercises);
    }
    public async Task<IEnumerable<ExerciseDto>> GetExercisesAsync(int planId)
    {
        var exercises = await _context.Exercises
            .Where(e => e.WorkoutPlanId == planId)
            .OrderBy(e => e.OrderIndex)
            .ToListAsync();
        return exercises.Select(e => new ExerciseDto
        {
            Id = e.Id,
            WorkoutPlanId = e.WorkoutPlanId,
            Name = e.Name,
            Description = e.Description,
            MuscleGroup = e.MuscleGroup,
            Equipment = e.Equipment,
            Sets = e.Sets,
            Reps = e.Reps,
            DurationMinutes = e.DurationMinutes,
            VideoUrl = e.VideoUrl
        });
    }
    private IEnumerable<Exercise> GenerateExercisesForGoalAndDifficulty(FitnessGoal goal, string difficulty, int workoutPlanId)
    {
        var exercises = new List<Exercise>();
        if (goal == FitnessGoal.WeightLoss)
        {
            exercises.AddRange(new[]
            {
                new Exercise { Name = "Squats", WorkoutPlanId = workoutPlanId, MuscleGroup = "Legs", Sets = 3, Reps = 12, OrderIndex = 1, CreatedAt = DateTime.UtcNow },
                new Exercise { Name = "Push-ups", WorkoutPlanId = workoutPlanId, MuscleGroup = "Chest", Sets = 3, Reps = 10, OrderIndex = 2, CreatedAt = DateTime.UtcNow },
                new Exercise { Name = "Running", WorkoutPlanId = workoutPlanId, MuscleGroup = "Cardio", DurationMinutes = 30, OrderIndex = 3, CreatedAt = DateTime.UtcNow }
            });
        }
        else if (goal == FitnessGoal.MuscleGain)
        {
            exercises.AddRange(new[]
            {
                new Exercise { Name = "Bench Press", WorkoutPlanId = workoutPlanId, MuscleGroup = "Chest", Sets = 4, Reps = 6, OrderIndex = 1, CreatedAt = DateTime.UtcNow },
                new Exercise { Name = "Deadlifts", WorkoutPlanId = workoutPlanId, MuscleGroup = "Back", Sets = 3, Reps = 5, OrderIndex = 2, CreatedAt = DateTime.UtcNow },
                new Exercise { Name = "Squats", WorkoutPlanId = workoutPlanId, MuscleGroup = "Legs", Sets = 4, Reps = 6, OrderIndex = 3, CreatedAt = DateTime.UtcNow }
            });
        }
        else
        {
            exercises.AddRange(new[]
            {
                new Exercise { Name = "Cardio", WorkoutPlanId = workoutPlanId, MuscleGroup = "Cardio", DurationMinutes = 45, OrderIndex = 1, CreatedAt = DateTime.UtcNow },
                new Exercise { Name = "Yoga", WorkoutPlanId = workoutPlanId, MuscleGroup = "Flexibility", DurationMinutes = 30, OrderIndex = 2, CreatedAt = DateTime.UtcNow }
            });
        }
        return exercises;
    }
    private static WorkoutPlanDto MapToDto(WorkoutPlan plan) => new()
    {
        Id = plan.Id,
        UserId = plan.UserId,
        Name = plan.Name,
        Description = plan.Description,
        DifficultyLevel = plan.DifficultyLevel.ToString(),
        DurationWeeks = plan.DurationWeeks,
        Exercises = plan.Exercises?.Select(e => new ExerciseDto
        {
            Id = e.Id,
            WorkoutPlanId = e.WorkoutPlanId,
            Name = e.Name,
            Description = e.Description,
            MuscleGroup = e.MuscleGroup,
            Equipment = e.Equipment,
            Sets = e.Sets,
            Reps = e.Reps,
            DurationMinutes = e.DurationMinutes,
            VideoUrl = e.VideoUrl
        }).ToList() ?? new List<ExerciseDto>()
    };
}
