using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
        public ICollection<NutritionLog> NutritionLogs { get; set; }
    }
}

