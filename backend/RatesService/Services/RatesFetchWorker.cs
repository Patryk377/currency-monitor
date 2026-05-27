using Microsoft.EntityFrameworkCore;
using RatesService.Data;
using RatesService.Models;

namespace RatesService.Services;

public class RatesFetchWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<RatesFetchWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(60);

    public RatesFetchWorker(IServiceProvider sp, ILogger<RatesFetchWorker> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await FetchOnceAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania kursów z NBP.");
            }
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task FetchOnceAsync(CancellationToken ct)
    {
        using var scope = _sp.CreateScope();
        var nbp = scope.ServiceProvider.GetRequiredService<NbpClient>();
        var db = scope.ServiceProvider.GetRequiredService<RatesDbContext>();

        var table = await nbp.GetTableAAsync(ct);
        if (table is null) return;   // weekend/święto - nic do zapisania

        int added = 0;
        foreach (var r in table.Rates)
        {
            bool exists = await db.ExchangeRates
                .AnyAsync(x => x.Code == r.Code && x.EffectiveDate == table.EffectiveDate, ct);
            if (exists) continue;

            db.ExchangeRates.Add(new ExchangeRate
            {
                Code = r.Code,
                Currency = r.Currency,
                Mid = r.Mid,
                EffectiveDate = table.EffectiveDate,
                FetchedAt = DateTime.UtcNow
            });
            added++;
        }

        await db.SaveChangesAsync(ct);
        _logger.LogInformation("Zapisano {Count} kursów z tabeli {No} ({Date}).",
            added, table.No, table.EffectiveDate);
    }
}