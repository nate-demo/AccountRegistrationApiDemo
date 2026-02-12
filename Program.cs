using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using AccountRegistrationApiDemo.Common.Middleware;
using AccountRegistrationApiDemo.Data;
using AccountRegistrationApiDemo.Services;

// ──────────────────────────────────────────────────────────────
// Builder Configuration
// ──────────────────────────────────────────────────────────────

var builder = WebApplication.CreateBuilder(args);

// --- Controllers + JSON serialization ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize enums as strings (e.g. "Active" instead of 0)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --- Swagger / OpenAPI ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Account Registration API Demo",
        Version = "v1",
        Description = "A lightweight, demo-friendly ASP.NET Core Web API that uses static JSON files " +
                      "as its data source. Perfect for training and learning REST API fundamentals. " +
                      "All data resets when the application restarts."
    });

    // Enable XML comments for Swagger documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// --- AutoMapper ---
// NOTE: AutoMapper v15+ requires a license key. Community licenses are free for
// companies / individuals with < $5M annual gross revenue.
// Register at https://automapper.io to obtain your key, then replace the placeholder below.
builder.Services.AddAutoMapper(cfg =>
{
    // Replace "YOUR-COMMUNITY-LICENSE-KEY" with a real key from https://automapper.io
    // For local training/demos, AutoMapper works without a key but will log a warning.
    // cfg.LicenseKey = "YOUR-COMMUNITY-LICENSE-KEY";
}, typeof(Program));

// --- FluentValidation ---
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// --- In-Memory Data Store (singleton — lives for the app's lifetime) ---
builder.Services.AddSingleton<InMemoryDataStore>();

// --- Application Services ---
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

// ──────────────────────────────────────────────────────────────
// App Configuration
// ──────────────────────────────────────────────────────────────

var app = builder.Build();

// --- Seed data from JSON files ---
using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<InMemoryDataStore>();
    await DataSeeder.SeedAsync(store, app.Environment.ContentRootPath);
}

// --- Global exception handling middleware ---
app.UseMiddleware<ExceptionHandlingMiddleware>();

// --- Swagger (available in all environments for this demo) ---
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Registration API v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Account Registration API Demo";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ──────────────────────────────────────────────────────────────
// Run
// ──────────────────────────────────────────────────────────────

app.Run();

// ──────────────────────────────────────────────────────────────
// Make Program accessible to integration tests
// ──────────────────────────────────────────────────────────────

public partial class Program { }
