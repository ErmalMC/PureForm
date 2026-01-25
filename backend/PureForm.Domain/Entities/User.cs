namespace PureForm.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public decimal Weight { get; set; } // in kg
    public decimal Height { get; set; } // in cm
    public string Gender { get; set; } = string.Empty;
    public string FitnessGoal { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    public decimal? DailyCalorieGoal { get; set; }
    public decimal? DailyProteinGoal { get; set; }
    public decimal? DailyCarbsGoal { get; set; }
    public decimal? DailyFatsGoal { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    public ICollection<NutritionLog> NutritionLogs { get; set; } = new List<NutritionLog>();
}