namespace AlertsService.Models;

public enum AlertDirection { Above, Below }   // próg w górę / w dół

public class Alert
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;        // "EUR"
    public decimal Threshold { get; set; }              // 4.40 - próg
    public AlertDirection Direction { get; set; }       // Above = "gdy przekroczy w górę"
    public bool IsTriggered { get; set; }               // czy już się wyzwolił
    public decimal? LastCheckedRate { get; set; }       // ostatni sprawdzony kurs
    public DateTime? TriggeredAt { get; set; }          // kiedy się wyzwolił
    public DateTime CreatedAt { get; set; }
}