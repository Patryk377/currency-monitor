using Microsoft.EntityFrameworkCore;
using AlertsService.Models;

namespace AlertsService.Data;

public class AlertsDbContext : DbContext
{
    public AlertsDbContext(DbContextOptions<AlertsDbContext> options) : base(options) { }

    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Alert>(e =>
        {
            e.Property(p => p.Code).HasMaxLength(3);
            e.Property(p => p.Threshold).HasPrecision(18, 4);
            e.Property(p => p.LastCheckedRate).HasPrecision(18, 4);
            e.Property(p => p.Direction).HasConversion<string>();  // enum zapisywany jako "Above"/"Below"
        });
    }
}