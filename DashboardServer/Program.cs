using System.Text;
using AutoMapper;
using DashboardServer.Data;
using DashboardServer.Filters;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services;
using DashboardServer.Services.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add CORS for Vue dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueDev",
        p => p.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? throw new InvalidOperationException("JWT Key not configured")))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c =>
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangfireConnection"))));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEncryptService, EncryptService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICanvasApiService, CanvasApiService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ICourseWeightService, CourseWeightService>();
builder.Services.AddScoped<IDailyCapacityService, DailyCapacityService>();
builder.Services.AddScoped<ISchedulerRulesService, SchedulerRulesService>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();

// Adding this to attempt injecting the DbContext with HttpContext so I can access the current user and create a global query filter.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserResolverService, UserResolverService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<ICourseWeightRepository, CourseWeightRepository>();
builder.Services.AddScoped<IDailyCapacityRepository, DailyCapacityRepository>();
builder.Services.AddScoped<ISchedulerRulesRepository, SchedulerRulesRepository>();
builder.Services.AddScoped<ISchedulerRepository, SchedulerRepository>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
}, typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("AllowVueDev");
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.UseHangfireDashboard();

// Exposes the /login /register endpoints implicitly
app.MapIdentityApi<IdentityUser>();

app.Run();
