namespace RatesService.Models;

// Koresponduje do struktury JSON z https://api.nbp.pl/api/exchangerates/tables/A
public record NbpTable(string Table, string No, DateOnly EffectiveDate, List<NbpRate> Rates);
public record NbpRate(string Currency, string Code, decimal Mid);