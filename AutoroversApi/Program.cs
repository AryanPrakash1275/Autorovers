using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Application.Abstractions.Behaviors; // for LoggingDecorator.*
using Autorovers.Infrastructure;
using Serilog;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// DI
var services = builder.Services;

services.AddControllers();

// Register your app/infrastructure services
// (AddInfrastructure already registers DbContext; no need to AddDbContext here)
services.AddInfrastructure(builder.Configuration);
services.AddAuthorization();

// If you don't already register handlers elsewhere, register them here (example):
// services.Scan(scan => scan
//     .FromApplicationDependencies(a => a.FullName!.StartsWith("Autorovers"))
//     .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>))).AsImplementedInterfaces().WithScopedLifetime()
//     .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
//     .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))).AsImplementedInterfaces().WithScopedLifetime()
// );

// Decorate AFTER handlers are registered
services.Decorate(typeof(ICommandHandler<,>),
    typeof(LoggingDecorator.CommandHandler<,>));

services.Decorate(typeof(ICommandHandler<>),
    typeof(LoggingDecorator.CommandBaseHandler<>));

services.Decorate(typeof(IQueryHandler<,>),
    typeof(LoggingDecorator.QueryHandler<,>));

var app = builder.Build();

// Pipeline

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
