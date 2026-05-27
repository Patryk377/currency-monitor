namespace RatesService.Models;

public class ExchangeRate
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;      // "EUR"
    public string Currency { get; set; } = default!;  // "euro"
    public decimal Mid { get; set; }                  // 4.2566 - kurs średni
    public DateOnly EffectiveDate { get; set; }        // data z tabeli NBP
    public DateTime FetchedAt { get; set; }            // kiedy my pobraliśmy
}