using PureForm.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Application.Interfaces
{
    public interface IExerciseService
    {
        Task<ExerciseDto?> GetByIdAsync(int id);
        Task<IEnumerable<ExerciseDto>> GetByWorkoutPlanIdAsync(int workoutPlanId);
        Task<ExerciseDto> CreateAsync(int workoutPlanId, CreateExerciseDto dto);
        Task<ExerciseDto?> UpdateAsync(int id, CreateExerciseDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ExerciseDto>> SearchExercisesAsync(string muscleGroup);
    }
}
