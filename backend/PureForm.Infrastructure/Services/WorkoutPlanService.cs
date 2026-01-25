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

    public async Task<WorkoutPlanDto?> GeneratePersonalizedPlanAsync(int userId, string? difficultyLevel = null)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return null;

        // Default to Intermediate if not specified
        var difficulty = difficultyLevel ?? "Intermediate";

        var planName = $"{difficulty} {user.FitnessGoal} Plan";
        var description = $"AI-generated {difficulty.ToLower()} workout plan customized for {user.FirstName}'s {user.FitnessGoal.ToLower()} goals";

        var plan = new WorkoutPlan
        {
            UserId = userId,
            Name = planName,
            Description = description,
            DifficultyLevel = difficulty,
            DurationWeeks = difficulty == "Beginner" ? 4 : difficulty == "Intermediate" ? 8 : 12,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _workoutPlanRepository.AddAsync(plan);

        var exercises = GenerateExercisesForGoalAndDifficulty(user.FitnessGoal, difficulty, created.Id);

        foreach (var exercise in exercises)
        {
            await _exerciseRepository.AddAsync(exercise);
        }


        var planWithExercises = await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .FirstOrDefaultAsync(wp => wp.Id == created.Id);

        return planWithExercises == null ? null : MapToDto(planWithExercises);
    }

    private List<Exercise> GenerateExercisesForGoalAndDifficulty(string fitnessGoal, string difficulty, int workoutPlanId)
    {
        var exercises = new List<Exercise>();
        var createdAt = DateTime.UtcNow;

        switch (fitnessGoal.ToLower())
        {
            case "muscle gain":
                exercises = difficulty switch
                {
                    "Beginner" => GenerateMuscleGainBeginnerExercises(workoutPlanId, createdAt),
                    "Intermediate" => GenerateMuscleGainIntermediateExercises(workoutPlanId, createdAt),
                    "Advanced" => GenerateMuscleGainAdvancedExercises(workoutPlanId, createdAt),
                    _ => GenerateMuscleGainIntermediateExercises(workoutPlanId, createdAt)
                };
                break;

            case "weight loss":
                exercises = difficulty switch
                {
                    "Beginner" => GenerateWeightLossBeginnerExercises(workoutPlanId, createdAt),
                    "Intermediate" => GenerateWeightLossIntermediateExercises(workoutPlanId, createdAt),
                    "Advanced" => GenerateWeightLossAdvancedExercises(workoutPlanId, createdAt),
                    _ => GenerateWeightLossIntermediateExercises(workoutPlanId, createdAt)
                };
                break;

            case "general fitness":
                exercises = difficulty switch
                {
                    "Beginner" => GenerateGeneralFitnessBeginnerExercises(workoutPlanId, createdAt),
                    "Intermediate" => GenerateGeneralFitnessIntermediateExercises(workoutPlanId, createdAt),
                    "Advanced" => GenerateGeneralFitnessAdvancedExercises(workoutPlanId, createdAt),
                    _ => GenerateGeneralFitnessIntermediateExercises(workoutPlanId, createdAt)
                };
                break;

            case "endurance":
                exercises = difficulty switch
                {
                    "Beginner" => GenerateEnduranceBeginnerExercises(workoutPlanId, createdAt),
                    "Intermediate" => GenerateEnduranceIntermediateExercises(workoutPlanId, createdAt),
                    "Advanced" => GenerateEnduranceAdvancedExercises(workoutPlanId, createdAt),
                    _ => GenerateEnduranceIntermediateExercises(workoutPlanId, createdAt)
                };
                break;

            default:
                exercises = GenerateGeneralFitnessBeginnerExercises(workoutPlanId, createdAt);
                break;
        }

        return exercises;
    }

    // ========== MUSCLE GAIN EXERCISES ==========
    private List<Exercise> GenerateMuscleGainBeginnerExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Dumbbell Bench Press", Description = "Build chest strength with controlled movement", MuscleGroup = "Chest", Equipment = "Dumbbells", Sets = 3, Reps = 10, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Goblet Squat", Description = "Learn proper squat form with a dumbbell", MuscleGroup = "Legs", Equipment = "Dumbbell", Sets = 3, Reps = 12, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Lat Pulldown", Description = "Build back strength with machine support", MuscleGroup = "Back", Equipment = "Machine", Sets = 3, Reps = 12, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Dumbbell Shoulder Press", Description = "Seated shoulder press for stability", MuscleGroup = "Shoulders", Equipment = "Dumbbells", Sets = 3, Reps = 10, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Leg Press", Description = "Safe leg strength builder", MuscleGroup = "Legs", Equipment = "Machine", Sets = 3, Reps = 12, OrderIndex = 5, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Dumbbell Bicep Curls", Description = "Isolate and build biceps", MuscleGroup = "Arms", Equipment = "Dumbbells", Sets = 3, Reps = 12, OrderIndex = 6, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Tricep Rope Pushdown", Description = "Cable tricep isolation", MuscleGroup = "Arms", Equipment = "Cable", Sets = 3, Reps = 12, OrderIndex = 7, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateMuscleGainIntermediateExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Bench Press", Description = "Classic chest builder for upper body strength", MuscleGroup = "Chest", Equipment = "Barbell", Sets = 4, Reps = 8, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Squat", Description = "King of leg exercises for overall lower body mass", MuscleGroup = "Legs", Equipment = "Barbell", Sets = 4, Reps = 8, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Deadlift", Description = "Full-body compound movement for strength and mass", MuscleGroup = "Back", Equipment = "Barbell", Sets = 4, Reps = 6, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Overhead Press", Description = "Build strong, muscular shoulders", MuscleGroup = "Shoulders", Equipment = "Barbell", Sets = 4, Reps = 8, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Row", Description = "Thick back development exercise", MuscleGroup = "Back", Equipment = "Barbell", Sets = 4, Reps = 10, OrderIndex = 5, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Romanian Deadlift", Description = "Hamstring and glute builder", MuscleGroup = "Legs", Equipment = "Barbell", Sets = 3, Reps = 10, OrderIndex = 6, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Curls", Description = "Mass building arm exercise", MuscleGroup = "Arms", Equipment = "Barbell", Sets = 3, Reps = 10, OrderIndex = 7, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateMuscleGainAdvancedExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Bench Press", Description = "Heavy pressing for maximum chest growth", MuscleGroup = "Chest", Equipment = "Barbell", Sets = 5, Reps = 5, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Incline Dumbbell Press", Description = "Upper chest development", MuscleGroup = "Chest", Equipment = "Dumbbells", Sets = 4, Reps = 8, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Barbell Squat", Description = "Heavy squats for leg mass", MuscleGroup = "Legs", Equipment = "Barbell", Sets = 5, Reps = 5, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Front Squat", Description = "Quad-focused squat variation", MuscleGroup = "Legs", Equipment = "Barbell", Sets = 4, Reps = 6, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Deadlift", Description = "Maximum strength and mass builder", MuscleGroup = "Back", Equipment = "Barbell", Sets = 5, Reps = 3, OrderIndex = 5, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Weighted Pull-ups", Description = "Advanced back and bicep builder", MuscleGroup = "Back", Equipment = "Pull-up Bar", Sets = 4, Reps = 6, OrderIndex = 6, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Overhead Press", Description = "Heavy shoulder pressing", MuscleGroup = "Shoulders", Equipment = "Barbell", Sets = 4, Reps = 6, OrderIndex = 7, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Dips (Weighted)", Description = "Chest and tricep mass builder", MuscleGroup = "Chest", Equipment = "Dip Bar", Sets = 4, Reps = 8, OrderIndex = 8, CreatedAt = createdAt },
        };
    }

    // ========== WEIGHT LOSS EXERCISES ==========
    private List<Exercise> GenerateWeightLossBeginnerExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Walking", Description = "Low-impact cardio to start your journey", MuscleGroup = "Full Body", Equipment = "None", Sets = 1, Reps = 1, DurationMinutes = 20, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Bodyweight Squats", Description = "Build leg strength and burn calories", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 15, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Modified Push-ups", Description = "Upper body strength from knees", MuscleGroup = "Chest", Equipment = "None", Sets = 3, Reps = 10, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Marching in Place", Description = "Gentle cardio warm-up", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 30, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Wall Push-ups", Description = "Beginner-friendly upper body exercise", MuscleGroup = "Chest", Equipment = "None", Sets = 3, Reps = 12, OrderIndex = 5, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Plank (Modified)", Description = "Core strength from knees", MuscleGroup = "Core", Equipment = "None", Sets = 3, Reps = 1, DurationMinutes = 1, OrderIndex = 6, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateWeightLossIntermediateExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Jumping Jacks", Description = "Full-body cardio warmup", MuscleGroup = "Full Body", Equipment = "None", Sets = 3, Reps = 30, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Burpees", Description = "High-intensity fat burning exercise", MuscleGroup = "Full Body", Equipment = "None", Sets = 4, Reps = 15, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Mountain Climbers", Description = "Core and cardio combination", MuscleGroup = "Core", Equipment = "None", Sets = 4, Reps = 20, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "High Knees", Description = "Cardio exercise to burn calories", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 30, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Jump Squats", Description = "Explosive leg exercise for fat loss", MuscleGroup = "Legs", Equipment = "None", Sets = 4, Reps = 15, OrderIndex = 5, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Plank", Description = "Core strengthening and stability", MuscleGroup = "Core", Equipment = "None", Sets = 3, Reps = 1, DurationMinutes = 1, OrderIndex = 6, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateWeightLossAdvancedExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "HIIT Sprints", Description = "Maximum calorie burn intervals", MuscleGroup = "Full Body", Equipment = "None", Sets = 8, Reps = 1, DurationMinutes = 1, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Burpee Box Jumps", Description = "Explosive full-body fat burner", MuscleGroup = "Full Body", Equipment = "Box", Sets = 5, Reps = 12, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Kettlebell Swings", Description = "Full-body power and cardio", MuscleGroup = "Full Body", Equipment = "Kettlebell", Sets = 5, Reps = 20, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Battle Ropes", Description = "Upper body and cardio endurance", MuscleGroup = "Full Body", Equipment = "Battle Ropes", Sets = 4, Reps = 1, DurationMinutes = 1, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Plyometric Push-ups", Description = "Explosive upper body power", MuscleGroup = "Chest", Equipment = "None", Sets = 4, Reps = 10, OrderIndex = 5, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Turkish Get-ups", Description = "Full-body functional movement", MuscleGroup = "Full Body", Equipment = "Kettlebell", Sets = 3, Reps = 5, OrderIndex = 6, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Thrusters", Description = "Compound cardio and strength", MuscleGroup = "Full Body", Equipment = "Dumbbells", Sets = 4, Reps = 15, OrderIndex = 7, CreatedAt = createdAt },
        };
    }

    // ========== GENERAL FITNESS EXERCISES ==========
    private List<Exercise> GenerateGeneralFitnessBeginnerExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Wall Push-ups", Description = "Beginner upper body strength", MuscleGroup = "Chest", Equipment = "None", Sets = 3, Reps = 12, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Chair Squats", Description = "Learn proper squat form safely", MuscleGroup = "Legs", Equipment = "Chair", Sets = 3, Reps = 15, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Standing Knee Raises", Description = "Core activation exercise", MuscleGroup = "Core", Equipment = "None", Sets = 3, Reps = 15, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Arm Circles", Description = "Shoulder mobility and warm-up", MuscleGroup = "Shoulders", Equipment = "None", Sets = 3, Reps = 20, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Seated Rows (Resistance Band)", Description = "Back strength with light resistance", MuscleGroup = "Back", Equipment = "Resistance Band", Sets = 3, Reps = 12, OrderIndex = 5, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateGeneralFitnessIntermediateExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Push-ups", Description = "Upper body strength and endurance", MuscleGroup = "Chest", Equipment = "None", Sets = 3, Reps = 15, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Bodyweight Squats", Description = "Lower body strength and mobility", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 20, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Lunges", Description = "Leg strength and balance", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 12, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Plank", Description = "Core stability exercise", MuscleGroup = "Core", Equipment = "None", Sets = 3, Reps = 1, DurationMinutes = 1, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Dumbbell Rows", Description = "Back and arm strength", MuscleGroup = "Back", Equipment = "Dumbbells", Sets = 3, Reps = 12, OrderIndex = 5, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateGeneralFitnessAdvancedExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "One-Arm Push-ups", Description = "Advanced upper body strength", MuscleGroup = "Chest", Equipment = "None", Sets = 3, Reps = 8, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Pistol Squats", Description = "Single-leg strength and balance", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 10, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Muscle-ups", Description = "Advanced pulling and pushing strength", MuscleGroup = "Full Body", Equipment = "Pull-up Bar", Sets = 3, Reps = 5, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Dragon Flags", Description = "Advanced core strength", MuscleGroup = "Core", Equipment = "Bench", Sets = 3, Reps = 8, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Handstand Push-ups", Description = "Advanced shoulder and arm strength", MuscleGroup = "Shoulders", Equipment = "Wall", Sets = 3, Reps = 8, OrderIndex = 5, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "L-Sit", Description = "Core and arm endurance", MuscleGroup = "Core", Equipment = "Parallettes", Sets = 3, Reps = 1, DurationMinutes = 1, OrderIndex = 6, CreatedAt = createdAt },
        };
    }

    // ========== ENDURANCE EXERCISES ==========
    private List<Exercise> GenerateEnduranceBeginnerExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Walking", Description = "Build base cardio endurance", MuscleGroup = "Legs", Equipment = "None", Sets = 1, Reps = 1, DurationMinutes = 20, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Light Jogging", Description = "Introduce running intervals", MuscleGroup = "Legs", Equipment = "None", Sets = 3, Reps = 1, DurationMinutes = 5, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Stationary Bike", Description = "Low-impact cardio", MuscleGroup = "Legs", Equipment = "Bike", Sets = 1, Reps = 1, DurationMinutes = 15, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Step-ups (Low Height)", Description = "Leg endurance builder", MuscleGroup = "Legs", Equipment = "Bench", Sets = 3, Reps = 15, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Elliptical", Description = "Full-body low-impact cardio", MuscleGroup = "Full Body", Equipment = "Elliptical", Sets = 1, Reps = 1, DurationMinutes = 15, OrderIndex = 5, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateEnduranceIntermediateExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Running", Description = "Build cardiovascular endurance", MuscleGroup = "Legs", Equipment = "None", Sets = 1, Reps = 1, DurationMinutes = 30, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Cycling", Description = "Low-impact cardio endurance", MuscleGroup = "Legs", Equipment = "Bike", Sets = 1, Reps = 1, DurationMinutes = 45, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Jump Rope", Description = "Cardio and coordination", MuscleGroup = "Full Body", Equipment = "Jump Rope", Sets = 5, Reps = 100, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Step-ups", Description = "Leg endurance and strength", MuscleGroup = "Legs", Equipment = "Bench", Sets = 4, Reps = 20, OrderIndex = 4, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Circuit Training", Description = "Full-body endurance circuit", MuscleGroup = "Full Body", Equipment = "Various", Sets = 3, Reps = 1, DurationMinutes = 20, OrderIndex = 5, CreatedAt = createdAt },
        };
    }

    private List<Exercise> GenerateEnduranceAdvancedExercises(int workoutPlanId, DateTime createdAt)
    {
        return new List<Exercise>
        {
            new() { WorkoutPlanId = workoutPlanId, Name = "Long Distance Running", Description = "Marathon-level endurance", MuscleGroup = "Legs", Equipment = "None", Sets = 1, Reps = 1, DurationMinutes = 60, OrderIndex = 1, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Hill Sprints", Description = "Power endurance training", MuscleGroup = "Legs", Equipment = "None", Sets = 10, Reps = 1, DurationMinutes = 1, OrderIndex = 2, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Rowing Machine", Description = "Full-body endurance", MuscleGroup = "Full Body", Equipment = "Rowing Machine", Sets = 1, Reps = 1, DurationMinutes = 45, OrderIndex = 3, CreatedAt = createdAt },
            new() { WorkoutPlanId = workoutPlanId, Name = "Swimming", Description = "Low-impact full-body endurance", MuscleGroup = "Full Body", Equipment = "Pool", Sets = 1, Reps = 1,DurationMinutes = 40, OrderIndex = 4, CreatedAt = createdAt },
new() { WorkoutPlanId = workoutPlanId, Name = "Assault Bike Intervals", Description = "High-intensity endurance", MuscleGroup = "Full Body", Equipment = "Assault Bike", Sets = 8, Reps = 1, DurationMinutes = 2, OrderIndex = 5, CreatedAt = createdAt },
new() { WorkoutPlanId = workoutPlanId, Name = "Burpee Marathon", Description = "Ultimate endurance test", MuscleGroup = "Full Body", Equipment = "None", Sets = 5, Reps = 50, OrderIndex = 6, CreatedAt = createdAt },
};
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