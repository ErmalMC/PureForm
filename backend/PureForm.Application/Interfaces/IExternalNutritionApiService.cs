using PureForm.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Application.Interfaces
{
    public interface IExternalNutritionApiService
    {
        Task<IEnumerable<NutritionLogDto>> SearchFoodAsync(string query);
        Task<NutritionLogDto?> GetFoodDetailsAsync(string foodId);
    }
}
