using PureForm.Domain.Common;
using PureForm.Domain.Enums;

namespace PureForm.Domain.Entities;

public class NutritionLog : BaseEntity
{
    public int UserId { get; set; }
    public DateTime LogDate { get; set; }
    public MealType MealType { get; set; }
    public string FoodName { get; set; } = string.Empty;
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fats { get; set; }
    public decimal ServingSize { get; set; }
    public string ServingUnit { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}
