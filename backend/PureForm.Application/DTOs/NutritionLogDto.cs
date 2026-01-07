using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Application.DTOs
{
    public class NutritionLogDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LogDate { get; set; }
        public string MealType { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fats { get; set; }
        public decimal ServingSize { get; set; }
        public string ServingUnit { get; set; } = string.Empty;
    }

    public class CreateNutritionLogDto
    {
        public DateTime LogDate { get; set; }
        public string MealType { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fats { get; set; }
        public decimal ServingSize { get; set; }
        public string ServingUnit { get; set; } = string.Empty;
    }
}
