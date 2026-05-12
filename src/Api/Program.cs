using Serilog;
using OpenTelemetry.Trace;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

// OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(b =>
        b.AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation()
         .AddConsoleExporter());

var app = builder.Build();

app.UseSerilogRequestLogging();

// Prometheus metrics endpoint
app.UseHttpMetrics();
app.MapMetrics();                // /metrics

app.MapControllers();
app.Run();
