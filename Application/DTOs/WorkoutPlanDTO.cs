using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class WorkoutPlanDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public Guid UserId { get; set; }
    }
}

