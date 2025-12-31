using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseMySql(
                                                            config.GetConnectionString("DefaultConnection"),
                                                            new MySqlServerVersion(new Version(8, 0, 44))
                                                        ));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<INutritionLogRepository, NutritionLogRepository>();

            return services;
        }
    }
}
