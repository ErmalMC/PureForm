using PureForm.Domain.Common;
using PureForm.Domain.Enums;

namespace PureForm.Domain.Entities;

public class WorkoutPlan : BaseEntity
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; }
    public int DurationWeeks { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
