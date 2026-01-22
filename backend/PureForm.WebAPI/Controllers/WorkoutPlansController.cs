using Microsoft.AspNetCore.Mvc;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;

namespace PureForm.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;

        public WorkoutPlansController(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutPlanDto>> GetById(int id)
        {
            var plan = await _workoutPlanService.GetByIdAsync(id);
            if (plan == null) return NotFound();
            return Ok(plan);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<WorkoutPlanDto>>> GetByUserId(int userId)
        {
            var plans = await _workoutPlanService.GetByUserIdAsync(userId);
            return Ok(plans);
        }

        [HttpPost("user/{userId}")]
        public async Task<ActionResult<WorkoutPlanDto>> Create(int userId, [FromBody] CreateWorkoutPlanDto dto)
        {
            var plan = await _workoutPlanService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = plan.Id }, plan);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkoutPlanDto>> Update(int id, [FromBody] CreateWorkoutPlanDto dto)
        {
            var plan = await _workoutPlanService.UpdateAsync(id, dto);
            if (plan == null) return NotFound();
            return Ok(plan);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _workoutPlanService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("user/{userId}/generate")]
        public async Task<ActionResult<WorkoutPlanDto>> GeneratePersonalized(
            int userId,
            [FromQuery] string? difficultyLevel = null)  // ADD THIS PARAMETER
        {
            var plan = await _workoutPlanService.GeneratePersonalizedPlanAsync(userId, difficultyLevel);
            if (plan == null) return NotFound("User not found");
            return Ok(plan);
        }
    }
}