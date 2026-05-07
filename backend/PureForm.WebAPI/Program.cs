using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using PureForm.Infrastructure.Data;
using PureForm.Infrastructure.Repositories;
using PureForm.Infrastructure.Services;
using PureForm.Application.Interfaces;
using PureForm.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use Railway's PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration - Support environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    connectionString = BuildPostgresConnectionString(databaseUrl);
}
else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PGHOST")))
{
    connectionString = $"Host={Environment.GetEnvironmentVariable("PGHOST")};" +
                      $"Port={Environment.GetEnvironmentVariable("PGPORT")};" +
                      $"Database={Environment.GetEnvironmentVariable("PGDATABASE")};" +
                      $"Username={Environment.GetEnvironmentVariable("PGUSER")};" +
                      $"Password={Environment.GetEnvironmentVariable("PGPASSWORD")};";
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string not configured.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INutritionLogService, NutritionLogService>();
builder.Services.AddScoped<INutritionCalculatorService, NutritionCalculatorService>();
builder.Services.AddScoped<IFoodItemService, FoodItemService>();

// JWT Authentication - Support environment variables
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
             ?? builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("JWT Key not configured");

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? builder.Configuration["Jwt:Issuer"];

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                  ?? builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtIssuer))
{
    throw new InvalidOperationException("JWT Issuer not configured");
}

if (string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("JWT Audience not configured");
}

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        ClockSkew = TimeSpan.Zero
    };
});

// CORS - Support environment variable for frontend URL
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                  "http://localhost:3000",
                  "http://localhost:5173",
                  "http://localhost:5174",
                  frontendUrl
              )
              .SetIsOriginAllowed(origin =>
              {
                  // Allow any Vercel deployment
                  if (origin.EndsWith(".vercel.app", StringComparison.OrdinalIgnoreCase))
                      return true;

                  // Allow configured origins
                  var allowedOrigins = new[]
                  {
                      "http://localhost:3000",
                      "http://localhost:5173",
                      "http://localhost:5174",
                      frontendUrl
                  };
                  return allowedOrigins.Contains(origin);
              })
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Add exception handling middleware early in the pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: CORS must come BEFORE other middleware
app.UseCors("AllowReactApp");

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            var foodService = scope.ServiceProvider.GetRequiredService<IFoodItemService>();
            await foodService.SeedPopularFoodsAsync();
            Console.WriteLine("Food items seeding completed.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
        throw;
    }
}

app.Run();

static string BuildPostgresConnectionString(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);

    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port > 0 ? uri.Port : 5432,
        Database = uri.AbsolutePath.TrimStart('/'),
        Username = userInfo.Length > 0 ? userInfo[0] : string.Empty,
        Password = userInfo.Length > 1 ? userInfo[1] : string.Empty
    };

    var query = uri.Query.TrimStart('?');
    if (!string.IsNullOrWhiteSpace(query))
    {
        foreach (var part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var kvp = part.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            if (kvp.Length == 0)
            {
                continue;
            }

            var key = kvp[0].ToLowerInvariant();
            var value = kvp.Length > 1 ? kvp[1] : string.Empty;

            if (key == "sslmode" && value.Equals("require", StringComparison.OrdinalIgnoreCase))
            {
                builder.SslMode = SslMode.Require;
            }

            if (key == "trustservercertificate" && value.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                builder.TrustServerCertificate = true;
            }
        }
    }

    return builder.ConnectionString;
}
