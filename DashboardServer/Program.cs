using AutoMapper;
using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEncryptService, EncryptService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

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
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

// Exposes the /login /register endpoints implicitly
app.MapIdentityApi<IdentityUser>();

app.Run();
