using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Domain.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public int WorkoutPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MuscleGroup { get; set; } = string.Empty;
        public string Equipment { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int? DurationMinutes { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public int OrderIndex { get; set; }
        public DateTime CreatedAt { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; } = null!;
    }
}
