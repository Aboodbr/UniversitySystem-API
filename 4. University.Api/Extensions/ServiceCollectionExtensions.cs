using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using University.Application.Interfaces;
using University.Application.Interfaces.Repositories;
using University.Application.Services;
using University.Infrastructure.Data;
using University.Infrastructure.Identity;
using University.Infrastructure.Repositories;

namespace University.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Database
        services.AddDbContext<UniversityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // 2. Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<UniversityDbContext>()
        .AddDefaultTokenProviders();

        // 3. JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

        // 4. Repositories & Unit of Work
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 5. Application Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>(); // Added for AuthController
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IGradingService, GradingService>();
        services.AddScoped<IGpaService, GpaService>();
        services.AddScoped<ICourseService, CourseService>(); // Added for Admin & Courses Controllers
        services.AddScoped<ISemesterService, SemesterService>(); // Added for Admin Controller
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IProfessorService, ProfessorService>();
        services.AddScoped<IStudentService, StudentService>();

        // 6. AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<University.Application.Mapping.AutoMapperProfile>());

        return services;
    }
}