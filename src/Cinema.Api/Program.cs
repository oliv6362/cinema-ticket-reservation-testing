using Cinema.Core.Interfaces;
using Cinema.Core.Services;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Time;

var builder = WebApplication.CreateBuilder(args);

// Add API/controller support
builder.Services.AddControllers();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application services
builder.Services.AddScoped<PricingService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// Infrastructure services
builder.Services.AddSingleton<ICinemaStore, InMemoryCinemaStore>();
builder.Services.AddScoped<ITimeProvider, SystemTimeProvider>();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

app.MapGet("/", () => Results.Redirect("/swagger"));

// Map controller endpoints
app.MapControllers();

app.Run();