namespace PureForm.Application.DTOs;

public class WorkoutPlanDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    /// <summary>String representation of DifficultyLevel enum for API serialization</summary>
    public string DifficultyLevel { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
    public List<ExerciseDto> Exercises { get; set; } = new();
}

public class CreateWorkoutPlanDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    /// <summary>String representation of DifficultyLevel enum</summary>
    public string DifficultyLevel { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
}

public class UpdateWorkoutPlanDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    /// <summary>String representation of DifficultyLevel enum</summary>
    public string? DifficultyLevel { get; set; }
    public int? DurationWeeks { get; set; }
}
