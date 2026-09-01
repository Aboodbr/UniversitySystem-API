using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using University.Api.Swagger;
using University.Api.Extensions;
using University.Api.Middlewares;
using University.Infrastructure.Data;
using University.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Configure Services (Dependency Injection)
// ==========================================

// Controllers & API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CORS Policy (Allowing Frontend applications to call the API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin(); // Can be restricted to specific domains in Production
    });
});

// Custom Application Services (Database, Identity, JWT, Repos, Application Services)
builder.Services.AddApplicationServices(builder.Configuration);

// Swagger Configuration with JWT support
builder.Services.AddSwaggerDocumentation();

// ==========================================
// 2. Build the Application Pipeline
// ==========================================
var app = builder.Build();
// ==========================================
// Seed Database on Startup
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<UniversityDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Call the initializer
        await DbInitializer.InitializeAsync(context, userManager, roleManager);
    }
    catch (System.Exception ex)
    {
        // Log errors if seeding fails
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}

// A. Global Exception Handler MUST be the very first middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// B. Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Enable Swagger UI and set it to load at the root URL (localhost:5186/)
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "University API v1"));
}

// C. Security & Routing Middlewares
app.UseHttpsRedirection();

// Use CORS before Authentication!
app.UseCors("AllowAll");

// D. Identity & Security (Order is critical: AuthN then AuthZ)
app.UseAuthentication(); // Verifies WHO you are (Validates JWT)
app.UseAuthorization();  // Verifies WHAT you can do (Checks Roles like "Admin")

// E. Map Endpoint Routing
app.MapControllers();

// ==========================================
// 3. Run the Application
// ==========================================
app.Run();