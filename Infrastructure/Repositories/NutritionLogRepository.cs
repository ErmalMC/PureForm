using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NutritionLogRepository : INutritionLogRepository
    {
        private readonly ApplicationDbContext _context;

        public NutritionLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NutritionLog log)
        {
            await _context.NutritionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var log = await _context.NutritionLogs.FindAsync(id);
            if (log != null)
            {
                _context.NutritionLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<NutritionLog> GetByIdAsync(Guid id)
        {
            return await _context.NutritionLogs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<NutritionLog>> GetByUserIdAsync(Guid userId)
        {
            return await _context.NutritionLogs
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(NutritionLog log)
        {
            _context.NutritionLogs.Update(log);
            await _context.SaveChangesAsync();
        }
    }
}
