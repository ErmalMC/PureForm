using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Application.DTOs
{
    public class WorkoutPlanDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public int DurationWeeks { get; set; }
        public List<ExerciseDto> Exercises { get; set; } = new();
    }

    public class CreateWorkoutPlanDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public int DurationWeeks { get; set; }
    }
}
