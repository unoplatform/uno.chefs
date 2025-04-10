namespace Chefs.Business.Models;

public record CategoryWithCount
{
	internal CategoryWithCount(int count, Category category)
	{
		Count = count;
		Category = category;
	}

	public int Count { get; init; }
	public Category Category { get; init; }
}
