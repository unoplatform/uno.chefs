namespace Chefs.Models.Entities;

public class SavedCookbooksData
{
	public Guid UserId { get; set; }
	public List<Guid>? SavedCookbooks { get; set; }
}
