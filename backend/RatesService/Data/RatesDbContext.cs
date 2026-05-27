using Microsoft.EntityFrameworkCore;
using RatesService.Models;

namespace RatesService.Data;

public class RatesDbContext : DbContext
{
    public RatesDbContext(DbContextOptions<RatesDbContext> options) : base(options) { }

    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<ExchangeRate>(e =>
        {
            e.Property(p => p.Code).HasMaxLength(3);
            e.Property(p => p.Mid).HasPrecision(18, 4);
            e.HasIndex(p => new { p.Code, p.EffectiveDate });
        });
    }
}