using Microsoft.EntityFrameworkCore;
using AlertsService.Data;
using AlertsService.Models;
using AlertsService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AlertsDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Adres RatesService - lokalnie localhost, w klastrze do nadpisania na http://rates-service:8080/
builder.Services.AddHttpClient<RatesClient>(c =>
    c.BaseAddress = new Uri(builder.Configuration["RatesService:BaseUrl"] ?? "http://localhost:5001/"));

builder.Services.AddHostedService<AlertCheckWorker>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AlertsDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors();

// Endpointy

// Lista alertów
app.MapGet("/api/alerts", async (AlertsDbContext db) =>
    Results.Ok(await db.Alerts.OrderByDescending(a => a.CreatedAt).ToListAsync()));

// Utwórz
app.MapPost("/api/alerts", async (AlertCreateDto dto, AlertsDbContext db) =>
{
    var alert = new Alert
    {
        Code = dto.Code.ToUpper(),
        Threshold = dto.Threshold,
        Direction = dto.Direction,
        CreatedAt = DateTime.UtcNow
    };
    db.Alerts.Add(alert);
    await db.SaveChangesAsync();
    return Results.Created($"/api/alerts/{alert.Id}", alert);
});

// Edytuj
app.MapPut("/api/alerts/{id:int}", async (int id, AlertUpdateDto dto, AlertsDbContext db) =>
{
    var alert = await db.Alerts.FindAsync(id);
    if (alert is null) return Results.NotFound();

    alert.Code = dto.Code.ToUpper();
    alert.Threshold = dto.Threshold;
    alert.Direction = dto.Direction;
    alert.IsTriggered = false;   // reset po edycji
    alert.TriggeredAt = null;
    await db.SaveChangesAsync();
    return Results.Ok(alert);
});

// Usuń
app.MapDelete("/api/alerts/{id:int}", async (int id, AlertsDbContext db) =>
{
    var alert = await db.Alerts.FindAsync(id);
    if (alert is null) return Results.NotFound();
    db.Alerts.Remove(alert);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();

record AlertCreateDto(string Code, decimal Threshold, AlertDirection Direction);
record AlertUpdateDto(string Code, decimal Threshold, AlertDirection Direction);