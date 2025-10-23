using AppDI = Autorovers.Application.DependencyInjection;
using InfraDI = Autorovers.Infrastructure.DependencyInjection;
using AutoroversApi.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Call static methods explicitly (no extension syntax, no ambiguity)
AppDI.AddApplication(builder.Services);
InfraDI.AddInfrastructure(builder.Services, builder.Configuration);

// MVC / auth
builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();

// Pipeline
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();

// Endpoints
app.MapHealth();
app.MapControllers();

app.Run();
