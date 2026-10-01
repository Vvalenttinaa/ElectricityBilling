using ElectricityBilling.API.Middleware;
using ElectricityBilling.Application.Interfaces;
using ElectricityBilling.Application.Services;
using ElectricityBilling.Infrastructure.Data;
using ElectricityBilling.Infrastructure.Persistence;
using ElectricityBilling.Infrastructure.Repositories;
using ElectricityBilling.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------
// CONTROLLERS
// -------------------------------------------------

builder.Services.AddControllers();

// -------------------------------------------------
// SWAGGER
// -------------------------------------------------

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// -------------------------------------------------
// DATABASE
// -------------------------------------------------

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "DefaultConnection is not configured.")));

// -------------------------------------------------
// DEPENDENCY INJECTION
// -------------------------------------------------

builder.Services.AddScoped<IPlanRepository, PlanRepository>();

builder.Services.AddScoped<
    ITaxGroupRepository,
    TaxGroupRepository>();

builder.Services.AddScoped<
    IRecommendationLogRepository,
    RecommendationLogRepository>();

builder.Services.AddScoped<
    IRecommendationService,
    RecommendationService>();

builder.Services.AddScoped<
    IPlanService,
    PlanService>();

builder.Services.AddScoped<
    ITaxGroupService,
    TaxGroupService>();

builder.Services.AddScoped<
    IAnalyticsService,
    AnalyticsService>();

builder.Services.AddScoped<
    IEmailService,
    EmailService>();

// -------------------------------------------------
// CORS
// -------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// -------------------------------------------------
// JWT AUTHENTICATION
// -------------------------------------------------

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT Key is not configured.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"];

var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    });

builder.Services.AddAuthorization();

// -------------------------------------------------
// BUILD APPLICATION
// -------------------------------------------------

var app = builder.Build();

// -------------------------------------------------
// EXCEPTION HANDLING
// -------------------------------------------------

app.UseMiddleware<ExceptionHandlingMiddleware>();

// -------------------------------------------------
// CORS
// -------------------------------------------------

app.UseCors("AllowAngular");

// -------------------------------------------------
// DATABASE MIGRATION + SEEDING
// -------------------------------------------------

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();

    var context =
        scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

    // Apply pending EF Core migrations.
    // This is important when starting with a new Docker PostgreSQL database.
    await context.Database.MigrateAsync();

    // Insert initial application data.
    await DatabaseSeeder.SeedAsync(context);
}

// -------------------------------------------------
// SWAGGER
// -------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Electricity Billing API v1");

        options.RoutePrefix = "swagger";
    });
}

// -------------------------------------------------
// HTTP PIPELINE
// -------------------------------------------------

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

// -------------------------------------------------
// REQUIRED FOR INTEGRATION TESTS
// -------------------------------------------------

public partial class Program
{
}