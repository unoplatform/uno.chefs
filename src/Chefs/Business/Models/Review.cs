using Chefs.Data;

namespace Chefs.Business;

public record Review
{
    public Review(ReviewData reviewData)
    {
        Score = reviewData.Score;
        Description = reviewData.Description;
    }

    public int Score { get; init; }
    public string? Description { get; init; }
}

