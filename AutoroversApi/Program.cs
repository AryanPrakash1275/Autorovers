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

// Call static methods explicitly (no extension syntax, no ambiguity)- not sure tho
AppDI.AddApplication(builder.Services);
InfraDI.AddInfrastructure(builder.Services, builder.Configuration);

// MVC & auth controllers. Still have to configure auth
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

// CORS CONFIG
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // i have hosted here
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

//swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pipeline
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowFrontend");

// Endpoints
app.MapHealth();
app.MapControllers();

app.Run();
