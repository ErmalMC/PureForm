namespace PureForm.Application.DTOs;

public class CalorieRecommendationDto
{
    public decimal BMR { get; set; } // Basal Metabolic Rate
    public decimal TDEE { get; set; } // Total Daily Energy Expenditure
    public decimal RecommendedCalories { get; set; }
    public decimal RecommendedProtein { get; set; }
    public decimal RecommendedCarbs { get; set; }
    public decimal RecommendedFats { get; set; }
    public string Explanation { get; set; } = string.Empty;
}