using Microsoft.EntityFrameworkCore;
using RatesService.Data;
using RatesService.Services;

var builder = WebApplication.CreateBuilder(args);

// Baza danych
builder.Services.AddDbContext<RatesDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Klient NBP (HttpClient z adresem bazowym)
builder.Services.AddHttpClient<NbpClient>(c =>
    c.BaseAddress = new Uri("https://api.nbp.pl/api/"));

// Worker pobierający kursy w tle 
builder.Services.AddHostedService<RatesFetchWorker>();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddOpenApi();

var app = builder.Build();

// Automatyczne migracje przy starcie 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RatesDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

// ENDPOINTY

// Najnowszy kurs każdej waluty
app.MapGet("/api/rates", async (RatesDbContext db) =>
{
    var latest = await db.ExchangeRates
        .GroupBy(r => r.Code)
        .Select(g => g.OrderByDescending(r => r.EffectiveDate).First())
        .ToListAsync();
    return Results.Ok(latest);
});

// Najnowszy kurs jednej waluty
app.MapGet("/api/rates/{code}", async (string code, RatesDbContext db) =>
{
    var rate = await db.ExchangeRates
        .Where(r => r.Code == code.ToUpper())
        .OrderByDescending(r => r.EffectiveDate)
        .FirstOrDefaultAsync();
    return rate is null ? Results.NotFound() : Results.Ok(rate);
});

// Historia kursu waluty
app.MapGet("/api/rates/{code}/history", async (string code, int days, RatesDbContext db) =>
{
    var span = days <= 0 ? 30 : days;
    var from = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-span));
    var history = await db.ExchangeRates
        .Where(r => r.Code == code.ToUpper() && r.EffectiveDate >= from)
        .OrderBy(r => r.EffectiveDate)
        .ToListAsync();
    return Results.Ok(history);
});

app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();