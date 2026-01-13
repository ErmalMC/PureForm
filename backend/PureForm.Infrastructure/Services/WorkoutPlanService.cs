using Microsoft.EntityFrameworkCore;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
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
        var plan = new WorkoutPlan
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
            DifficultyLevel = dto.DifficultyLevel,
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
        plan.DifficultyLevel = dto.DifficultyLevel;
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

    public async Task<WorkoutPlanDto?> GeneratePersonalizedPlanAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return null;

        // Create personalized plan based on fitness goal
        var planName = $"Personalized {user.FitnessGoal} Plan";
        var description = $"AI-generated workout plan customized for {user.FirstName}'s {user.FitnessGoal.ToLower()} goals";
        var difficultyLevel = "Intermediate";

        var plan = new WorkoutPlan
        {
            UserId = userId,
            Name = planName,
            Description = description,
            DifficultyLevel = difficultyLevel,
            DurationWeeks = 8,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _workoutPlanRepository.AddAsync(plan);

        // Generate exercises based on fitness goal
        var exercises = GenerateExercisesForGoal(user.FitnessGoal, created.Id);

        foreach (var exercise in exercises)
        {
            await _exerciseRepository.AddAsync(exercise);
        }

        // Reload plan with exercises
        var planWithExercises = await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .FirstOrDefaultAsync(wp => wp.Id == created.Id);

        return planWithExercises == null ? null : MapToDto(planWithExercises);
    }

    private List<Exercise> GenerateExercisesForGoal(string fitnessGoal, int workoutPlanId)
    {
        var exercises = new List<Exercise>();
        var createdAt = DateTime.UtcNow;

        switch (fitnessGoal.ToLower())
        {
            case "muscle gain":
                exercises = new List<Exercise>
                {
                    new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Bench Press", Description = "Classic chest builder for upper body strength", MuscleGroup = "Chest", Equipment = "Barbell", Sets = 4, Reps = 8, OrderIndex = 1, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Squat", Description = "King of leg exercises for overall lower body mass", MuscleGroup = "Legs", Equipment = "Barbell", Sets = 4, Reps = 8, OrderIndex = 2, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Deadlift", Description = "Full-body compound movement for strength and mass", MuscleGroup = "Back", Equipment = "Barbell", Sets = 4, Reps = 6, OrderIndex = 3, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Overhead Press", Description = "Build strong, muscular shoulders", MuscleGroup = "Shoulders", Equipment = "Barbell", Sets = 4, Reps = 8, OrderIndex = 4, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Row", Description = "Thick back development exercise", MuscleGroup = "Back", Equipment = "Barbell", Sets = 4, Reps = 10, OrderIndex = 5, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Dumbbell Curls", Description = "Isolate and build the biceps", MuscleGroup = "Arms", Equipment = "Dumbbells", Sets = 3, Reps = 12, OrderIndex = 6, CreatedAt = createdAt },
                };
                break;

            case "weight loss":
                exercises = new List<Exercise>
                {
                    new() { WorkoutPlanId = workoutPlanId, Name = "Jumping Jacks", Description = "Full-body cardio warmup", MuscleGroup = "Full Body", Equipment = "None", Sets = 3, Reps = 30, OrderIndex = 1, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Burpees", Description = "High-intensity fat burning exercise", MuscleGroup = "Full Body", Equipment = "None", Sets = 4, Reps = 15, OrderIndex = 2, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Mountain Climbers", Description = "Core and cardio combination", MuscleGroup = "Core", Equipment = "None", Sets = 4, Reps = 20, OrderIndex = 3, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "High Knees", Description = "Cardio exercise to burn calories", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 30, OrderIndex = 4, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Jump Squats", Description = "Explosive leg exercise for fat loss", MuscleGroup = "Legs", Equipment = "None", Sets = 4, Reps = 15, OrderIndex = 5, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Plank", Description = "Core strengthening and stability", MuscleGroup = "Core", Equipment = "None", Sets = 3, Reps = 1, DurationMinutes = 1, OrderIndex = 6, CreatedAt = createdAt },
                };
                break;

            case "general fitness":
                exercises = new List<Exercise>
                {
                    new() { WorkoutPlanId = workoutPlanId, Name = "Push-ups", Description = "Upper body strength and endurance", MuscleGroup = "Chest", Equipment = "None", Sets = 3, Reps = 15, OrderIndex = 1, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Bodyweight Squats", Description = "Lower body strength and mobility", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 20, OrderIndex = 2, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Lunges", Description = "Leg strength and balance", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 12, OrderIndex = 3, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Plank", Description = "Core stability exercise", MuscleGroup = "Core", Equipment = "None", Sets = 3, Reps = 1, DurationMinutes = 1, OrderIndex = 4, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Dumbbell Rows", Description = "Back and arm strength", MuscleGroup = "Back", Equipment = "Dumbbells", Sets = 3, Reps = 12, OrderIndex = 5, CreatedAt = createdAt },
                };
                break;

            case "endurance":
                exercises = new List<Exercise>
                {
                    new() { WorkoutPlanId = workoutPlanId, Name = "Running", Description = "Build cardiovascular endurance", MuscleGroup = "Legs", Equipment = "None", Sets = 1, Reps = 1, DurationMinutes = 30, OrderIndex = 1, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Cycling", Description = "Low-impact cardio endurance", MuscleGroup = "Legs", Equipment = "Bike", Sets = 1, Reps = 1, DurationMinutes = 45, OrderIndex = 2, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Jump Rope", Description = "Cardio and coordination", MuscleGroup = "Full Body", Equipment = "Jump Rope", Sets = 5, Reps = 100, OrderIndex = 3, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Step-ups", Description = "Leg endurance and strength", MuscleGroup = "Legs", Equipment = "Bench", Sets = 4, Reps = 20, OrderIndex = 4, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Circuit Training", Description = "Full-body endurance circuit", MuscleGroup = "Full Body", Equipment = "Various", Sets = 3, Reps = 1, DurationMinutes = 20, OrderIndex = 5, CreatedAt = createdAt },
                };
                break;

            default:
                // Default general fitness plan
                exercises = new List<Exercise>
                {
                    new() { WorkoutPlanId = workoutPlanId, Name = "Push-ups", Description = "Upper body strength", MuscleGroup = "Chest", Equipment = "None", Sets = 3, Reps = 15, OrderIndex = 1, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Squats", Description = "Lower body strength", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 20, OrderIndex = 2, CreatedAt = createdAt },
                    new() { WorkoutPlanId = workoutPlanId, Name = "Plank", Description = "Core strength", MuscleGroup = "Core", Equipment = "None", Sets = 3, Reps = 1, DurationMinutes = 1, OrderIndex = 3, CreatedAt = createdAt },
                };
                break;
        }

        return exercises;
    }

    private static WorkoutPlanDto MapToDto(WorkoutPlan plan) => new()
    {
        Id = plan.Id,
        UserId = plan.UserId,
        Name = plan.Name,
        Description = plan.Description,
        DifficultyLevel = plan.DifficultyLevel,
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
            VideoUrl = e.VideoUrl,
            ImageUrl = e.ImageUrl
        }).ToList() ?? new List<ExerciseDto>()
    };
}