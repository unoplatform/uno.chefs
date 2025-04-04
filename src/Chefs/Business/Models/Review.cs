using ReviewData = Chefs.Services.Clients.Models.ReviewData;

namespace Chefs.Business.Models;

public partial record Review
{
	public Review(ReviewData reviewData)
	{
		Id = reviewData.Id ?? Guid.NewGuid();
		RecipeId = reviewData.RecipeId ?? Guid.Empty;
		CreatedBy = reviewData.CreatedBy ?? Guid.Empty;
		PublisherName = reviewData.PublisherName;
		Date = reviewData.Date?.DateTime ?? DateTime.MinValue;
		Likes = reviewData.Likes?.Where(g=>g.HasValue).Select(g=>g.Value).ToImmutableList() ?? ImmutableList<Guid>.Empty;
		Dislikes = reviewData.Dislikes?.Where(g => g.HasValue).Select(g => g.Value).ToImmutableList() ?? ImmutableList<Guid>.Empty;
		Description = reviewData.Description;
		UrlAuthorImage = reviewData.UrlAuthorImage;
		UserLike = reviewData.UserLike ?? false;
	}
	
	public Review(Guid recipeId, string text)
	{
		Id = Guid.NewGuid();
		RecipeId = recipeId;
		Description = text;
	}
	
	public Guid Id { get; init; }
	public Guid RecipeId { get; init; }
	public string? UrlAuthorImage { get; init; }
	public Guid CreatedBy { get; init; }
	public string? PublisherName { get; init; }
	public DateTime Date { get; init; }
	public string? Description { get; init; }
	public ImmutableList<Guid>? Likes { get; init; }
	public ImmutableList<Guid>? Dislikes { get; init; }
	public bool UserLike { get; init; }
	
	internal ReviewData ToData() => new()
	{
		Id = Id,
		RecipeId = RecipeId,
		CreatedBy = CreatedBy,
		PublisherName = PublisherName,
		Date = Date,
		Likes = Likes?.Cast<Guid?>().ToList(), 
		Dislikes = Dislikes?.Cast<Guid?>().ToList(),
		Description = Description,
		UrlAuthorImage = UrlAuthorImage,
		UserLike = UserLike
	};
}
