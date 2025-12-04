using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetByIdAsync(Guid id);
        Task<IEnumerable<Exercise>> GetByWorkoutPlanIdAsync(Guid planId);
        Task AddAsync(Exercise exercise);
        Task UpdateAsync(Exercise exercise);
        Task DeleteAsync(Guid id);
    }
}
