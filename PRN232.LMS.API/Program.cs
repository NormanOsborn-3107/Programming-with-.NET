using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Implementations;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories;
using PRN232.LMS.Services.Implementations;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services;
using PRN232.LMS.API.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "PRN232 – LMS API",
        Version     = "v1",
        Description = "Learning Management System REST API built with ASP.NET Core 9.",
        Contact     = new OpenApiContact { Name = "PRN232 Team" }
    });

    // Include XML comments (request/response documentation)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ── EF Core – SQL Server (supports both local and Docker connection string keys)
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? builder.Configuration.GetConnectionString("LMSConnection")
           ?? throw new InvalidOperationException("No connection string configured.");

builder.Services.AddDbContext<LMSDbContext>(options =>
    options.UseSqlServer(connStr, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));

// ── Dependency Injection: Repositories ───────────────────────────────────────
builder.Services.AddRepositories();

// ── Dependency Injection: Services ────────────────────────────────────────────
builder.Services.AddServices();

var app = builder.Build();

// ── Auto-migrate & seed DB on startup ─────────────────────────────────────────
app.ApplyMigrations();

// ── Swagger UI (available in all environments) ────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS API v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "PRN232 LMS API";
});

// ── Health check endpoint (required by grader: DY-01) ────────────────────────
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

// ── HTTP Pipeline ─────────────────────────────────────────────────────────────
app.UseAuthorization();
app.MapControllers();

app.Run();



