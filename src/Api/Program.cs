using Microsoft.EntityFrameworkCore;
using Serilog;
using OpenTelemetry.Trace;
using Prometheus;
using PDH.Shared.Infrastructure;
using PDH.Shared.Infrastructure.Extensions;
using PDH.Application;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddOpenTelemetry()
    .WithTracing(b =>
        b.AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation()
         .AddConsoleExporter());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpMetrics();
app.MapMetrics();
app.MapControllers();
app.Run();
