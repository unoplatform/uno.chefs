namespace Chefs.Data;

public record ReviewData
{
    public int Score { get; init; }
    public string? Description { get; init; }
}
