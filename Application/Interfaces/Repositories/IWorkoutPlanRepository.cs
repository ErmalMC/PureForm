using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IWorkoutPlanRepository
    {
        Task<WorkoutPlan> GetByIdAsync(Guid id);
        Task<IEnumerable<WorkoutPlan>> GetByUserIdAsync(Guid userId);
        Task AddAsync(WorkoutPlan plan);
        Task UpdateAsync(WorkoutPlan plan);
        Task DeleteAsync(Guid id);
    }
}
