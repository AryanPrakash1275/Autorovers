using AppDI = Autorovers.Application.DependencyInjection;
using InfraDI = Autorovers.Infrastructure.DependencyInjection;

using AutoroversApi.Endpoints;
using AutoroversApi.Auth;
using Autorovers.Infrastructure.Persistence.Context;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Optional, helps with some timestamp behavior in Npgsql
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Detect Render platform
var isRender = Environment.GetEnvironmentVariable("RENDER") == "true";

// ========== Serilog ==========
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ========== Application & Infrastructure ==========
AppDI.AddApplication(builder.Services);
InfraDI.AddInfrastructure(builder.Services, builder.Configuration);

// ========== JWT AUTH CONFIG ==========
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // dev; Render will be behind HTTPS anyway
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            RoleClaimType = ClaimTypes.Role
        };
    });

// ========== AUTHORIZATION POLICIES ==========
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

// ========== MVC / Swagger ==========
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        // This removes all "field": null from your API responses
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AutoroversApi",
        Version = "v1"
    });

    // ---- JWT Bearer for Swagger ----
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// ========== CORS (relaxed for initial deploy) ==========
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCors", cors =>
    {
        cors
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ========== APPLY DB MIGRATIONS ON STARTUP ==========
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AutoroversDbContext>();
    db.Database.Migrate();
}

// ========== AUTH LOGIN ENDPOINT ==========
app.MapPost("/api/auth/login", (LoginRequest login, IConfiguration config) =>
{
    var adminSection = config.GetSection("Admin");
    var adminEmail = adminSection["Email"] ?? string.Empty;
    var adminPassword = adminSection["Password"] ?? string.Empty;

    if (!string.Equals(login.Email, adminEmail, StringComparison.OrdinalIgnoreCase) ||
        login.Password != adminPassword)
    {
        return Results.Unauthorized();
    }

    var jwtSection = config.GetSection("Jwt");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, adminEmail),
        new Claim(ClaimTypes.Name, adminEmail),
        new Claim(ClaimTypes.Role, "Admin")
    };

    var expires = DateTime.UtcNow.AddMinutes(
        int.TryParse(jwtSection["ExpiryMinutes"], out var mins) ? mins : 60);

    var token = new JwtSecurityToken(
        issuer: jwtSection["Issuer"],
        audience: jwtSection["Audience"],
        claims: claims,
        expires: expires,
        signingCredentials: creds
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new
    {
        token = tokenString,
        expiresAt = expires
    });
});

// ========== Swagger ==========
if (app.Environment.IsDevelopment() || isRender)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ========== PIPELINE ==========
app.UseSerilogRequestLogging();

app.UseCors("FrontendCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapHealth();
app.MapControllers();

app.Run();
