using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkoutPlan
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Exercise> Exercises { get; set; }
    }
}

