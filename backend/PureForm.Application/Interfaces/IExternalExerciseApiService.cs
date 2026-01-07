using PureForm.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Application.Interfaces
{
    public interface IExternalExerciseApiService
    {
        Task<IEnumerable<ExerciseDto>> GetExercisesByMuscleGroupAsync(string muscleGroup);
        Task<ExerciseDto?> GetExerciseByIdAsync(string externalId);
    }
}
