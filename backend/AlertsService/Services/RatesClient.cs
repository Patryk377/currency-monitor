namespace AlertsService.Services;

public record RateDto(string Code, string Currency, decimal Mid);

public class RatesClient
{
    private readonly HttpClient _http;
    private readonly ILogger<RatesClient> _logger;

    public RatesClient(HttpClient http, ILogger<RatesClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<decimal?> GetMidAsync(string code, CancellationToken ct)
    {
        var resp = await _http.GetAsync($"api/rates/{code}", ct);
        if (!resp.IsSuccessStatusCode)
        {
            _logger.LogWarning("RatesService zwrócił {Status} dla {Code}.", resp.StatusCode, code);
            return null;
        }
        var dto = await resp.Content.ReadFromJsonAsync<RateDto>(cancellationToken: ct);
        return dto?.Mid;
    }
}