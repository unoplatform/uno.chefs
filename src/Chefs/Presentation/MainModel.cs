
namespace Chefs.Presentation;

public partial class MainModel // DR_REV: Use Model suffix instead of ViewModel
{
	private readonly INavigator _navigator;

	public string? Title { get; }

	public MainModel(
		INavigator navigator,
		IOptions<AppConfig> appInfo)
	{ 
	
		_navigator = navigator;
		Title = $"Main - {appInfo?.Value?.Title}";
	}
}
