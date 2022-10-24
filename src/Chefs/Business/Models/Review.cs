using Chefs.Data;
using System.Net;
using System.Xml.Linq;
using Windows.System;

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

    internal ReviewData ToData() => new()
    {
        Score = Score,
        Description = Description
    };
}

