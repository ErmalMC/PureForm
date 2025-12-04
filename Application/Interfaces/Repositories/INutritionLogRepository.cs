using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface INutritionLogRepository
    {
        Task<NutritionLog> GetByIdAsync(Guid id);
        Task<IEnumerable<NutritionLog>> GetByUserIdAsync(Guid userId);
        Task AddAsync(NutritionLog log);
        Task UpdateAsync(NutritionLog log);
        Task DeleteAsync(Guid id);
    }
}
