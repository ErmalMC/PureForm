using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Data;
using PureForm.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace PureForm.Infrastructure.Services
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly IRepository<WorkoutPlan> _workoutPlanRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ApplicationDbContext _context;

        public WorkoutPlanService(
            IRepository<WorkoutPlan> workoutPlanRepository,
            IRepository<User> userRepository,
            ApplicationDbContext context)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _userRepository = userRepository;
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

            var planName = $"Personalized {user.FitnessGoal} Plan";
            var description = $"Customized workout plan for {user.FirstName}";
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
            return MapToDto(created);
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
}
