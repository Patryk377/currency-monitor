using Microsoft.EntityFrameworkCore;
using AlertsService.Data;
using AlertsService.Models;

namespace AlertsService.Services;

public class AlertCheckWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<AlertCheckWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

    public AlertCheckWorker(IServiceProvider sp, ILogger<AlertCheckWorker> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try { await CheckAllAsync(stoppingToken); }
            catch (Exception ex) { _logger.LogError(ex, "Błąd sprawdzania alertów."); }
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CheckAllAsync(CancellationToken ct)
    {
        using var scope = _sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AlertsDbContext>();
        var rates = scope.ServiceProvider.GetRequiredService<RatesClient>();

        var active = await db.Alerts.Where(a => !a.IsTriggered).ToListAsync(ct);

        foreach (var alert in active)
        {
            var mid = await rates.GetMidAsync(alert.Code, ct);
            if (mid is null) continue;

            alert.LastCheckedRate = mid;

            bool hit = alert.Direction == AlertDirection.Above
                ? mid >= alert.Threshold
                : mid <= alert.Threshold;

            if (hit)
            {
                alert.IsTriggered = true;
                alert.TriggeredAt = DateTime.UtcNow;
                _logger.LogInformation("ALERT: {Code} = {Mid} (próg {Dir} {Threshold}).",
                    alert.Code, mid, alert.Direction, alert.Threshold);
            }
        }
        await db.SaveChangesAsync(ct);
    }
}