using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Exercise
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MuscleGroup { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }

        public Guid WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
    }
}

