using AppDI = Autorovers.Application.DependencyInjection;
using InfraDI = Autorovers.Infrastructure.DependencyInjection;

using AutoroversApi.Endpoints;
using AutoroversApi.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
        // Dev-only: allow HTTP. In real prod behind HTTPS, set this true.
        options.RequireHttpsMetadata = false;
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

            // Ensure role-based checks use the standard role claim
            RoleClaimType = ClaimTypes.Role
        };
    });

// ========== AUTHORIZATION POLICIES ==========
builder.Services.AddAuthorization(options =>
{
    // This matches [Authorize(Policy = "AdminOnly")]
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

// ========== MVC / Swagger ==========
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== CORS ==========
var frontendUrls = builder.Configuration
    .GetSection("Frontend:Urls")
    .Get<string[]>() ?? new[] { "http://localhost:5173", "http://localhost:4173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCors", cors =>
    {
        cors
            .WithOrigins(frontendUrls)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ========== AUTH LOGIN ENDPOINT ==========
app.MapPost("/api/auth/login", (LoginRequest login, IConfiguration config) =>
{
    var adminSection = config.GetSection("Admin");
    var adminEmail = adminSection["Email"] ?? string.Empty;
    var adminPassword = adminSection["Password"] ?? string.Empty;

    // Single admin user from config
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
if (app.Environment.IsDevelopment())
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
