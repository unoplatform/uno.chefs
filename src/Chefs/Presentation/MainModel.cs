namespace Chefs.Presentation;

public partial record MainModel
{
	public string? Title { get; }

	public MainModel(IOptions<AppConfig> appInfo)
	{
		Title = $"Main - {appInfo?.Value?.Title ?? "Chefs"}";
	}
}
