using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkoutPlanRepository : IWorkoutPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WorkoutPlan plan)
        {
            await _context.WorkoutPlans.AddAsync(plan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var plan = await _context.WorkoutPlans.FindAsync(id);
            if (plan != null)
            {
                _context.WorkoutPlans.Remove(plan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<WorkoutPlan> GetByIdAsync(Guid id)
        {
            return await _context.WorkoutPlans
                .Include(p => p.Exercises)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<WorkoutPlan>> GetByUserIdAsync(Guid userId)
        {
            return await _context.WorkoutPlans
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(WorkoutPlan plan)
        {
            _context.WorkoutPlans.Update(plan);
            await _context.SaveChangesAsync();
        }
    }
}
