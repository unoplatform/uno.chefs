
namespace Chefs.Presentation;

public partial class MainViewModel // DR_REV: Use Model suffix instead of ViewModel
{
	private readonly INavigator _navigator;

	public string? Title { get; }

	public MainViewModel(
		INavigator navigator,
		IOptions<AppConfig> appInfo)
	{ 
	
		_navigator = navigator;
		Title = $"Main - {appInfo?.Value?.Title}";
	}

	public async Task GoToSecond(CancellationToken cancellation)
	{
		await _navigator.NavigateViewModelAsync<SecondViewModel>(this, cancellation: cancellation);
	}
}
