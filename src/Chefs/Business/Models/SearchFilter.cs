using Chefs.Data;

namespace Chefs.Business;

public record SearchFilter
{
    public OrganizeCategories OrganizeCategories { get; init; }
    public Times Times { get; init; }
    public Difficulties Difficulty { get; init; }
    public Category? Category { get; init; }
    public string? TextFilter { get; init; }
}
