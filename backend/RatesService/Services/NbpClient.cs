using RatesService.Models;

namespace RatesService.Services;

public class NbpClient
{
    private readonly HttpClient _http;
    private readonly ILogger<NbpClient> _logger;

    public NbpClient(HttpClient http, ILogger<NbpClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    // Zwraca null, gdy tabela nie jest opublikowana (404 — weekend/święto)
    public async Task<NbpTable?> GetTableAAsync(CancellationToken ct)
    {
        var resp = await _http.GetAsync("exchangerates/tables/A/?format=json", ct);

        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogInformation("NBP: tabela A niedostępna (404) — prawdopodobnie weekend/święto.");
            return null;
        }

        resp.EnsureSuccessStatusCode();
        var tables = await resp.Content.ReadFromJsonAsync<List<NbpTable>>(cancellationToken: ct);
        return tables?.FirstOrDefault();
    }
}