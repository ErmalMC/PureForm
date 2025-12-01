using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ExerciseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public Guid WorkoutPlanId { get; set; }
    }
}

