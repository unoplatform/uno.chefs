namespace Chefs.Data;

public record  NotificationData
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public bool Read { get; init; }
    public DateTime Date { get; init; }
}
