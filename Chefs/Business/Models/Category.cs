using CategoryData = Chefs.Services.Clients.Models.CategoryData;
namespace Chefs.Business.Models;

public partial record Category
{
	internal Category(CategoryData? category)
	{
		Id = category?.Id;
		UrlIcon = category?.UrlIcon;
		Name = category?.Name;
		Color = category?.Color;
	}

	public int? Id { get; init; }
	public string? UrlIcon { get; init; }
	public string? Name { get; init; }
	public string? Color { get; init; }

	internal CategoryData ToData() => new()
	{
		Id = Id,
		UrlIcon = UrlIcon,
		Name = Name
	};
}
