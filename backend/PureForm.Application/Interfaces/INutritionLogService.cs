using PureForm.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Application.Interfaces
{
    public interface INutritionLogService
    {
        Task<NutritionLogDto?> GetByIdAsync(int id);
        Task<IEnumerable<NutritionLogDto>> GetByUserIdAsync(int userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<NutritionLogDto> CreateAsync(int userId, CreateNutritionLogDto dto);
        Task<NutritionLogDto?> UpdateAsync(int id, CreateNutritionLogDto dto);
        Task<bool> DeleteAsync(int id);
        Task<Dictionary<string, decimal>> GetDailyTotalsAsync(int userId, DateTime date);
    }
}
