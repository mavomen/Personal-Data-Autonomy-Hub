using PDH.Api.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using OpenTelemetry.Trace;
using Prometheus;
using PDH.Shared.Infrastructure;
using PDH.Shared.Infrastructure.Extensions;
using PDH.Application;
using PDH.Modules.Integrations;
using PDH.Application.Interfaces;
using PDH.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Enable automatic ProblemDetails for invalid model state
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddProblemDetails();

// OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(b =>
        b.AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation()
         .AddConsoleExporter());

// EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Application & Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddIntegrations();
builder.Services.AddInfrastructure();

// JWT Authentication
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDataProtection();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddSignalR();
}

var app = builder.Build();

// Global error handling middleware (catch unhandled exceptions and return ProblemDetails)
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred",
            Detail = "Please try again later or contact support.",
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.UseSerilogRequestLogging();
app.UseHttpMetrics();
app.MapMetrics();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map the SignalR hub only when not in testing
if (!app.Environment.IsEnvironment("Testing"))
{
    app.MapHub<NotificationHub>("/hubs/notifications");
}

app.Run();

public partial class Program { }
