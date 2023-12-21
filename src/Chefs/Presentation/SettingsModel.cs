using Uno.Extensions.Toolkit;
using AppTheme = Uno.Extensions.Toolkit.AppTheme;

namespace Chefs.Presentation;

public partial class SettingsModel
{
	private readonly IUserService _userService;
	private readonly IThemeService _themeService;

	public SettingsModel(
		IThemeService themeService,
		INavigator navigator,
		IUserService userService,
		User user)
	{
		_userService = userService;
		_themeService = themeService;

		Profile = State.Value(this, () => user);

		Settings.ForEachAsync(async (settings, ct) =>
		{
			await _themeService.SetThemeAsync((settings?.IsDark ?? false) ? AppTheme.Dark : AppTheme.Light);
			await _userService.SetSettings((await Settings)!, ct);
		});

		Profile.ForEachAsync(async (profile, ct) =>
		{
			if (profile is null)
			{
				return;
			}

			await _userService.Update(profile, ct);
		});
	}

	public IState<User> Profile { get; }

	public IState<AppConfig> Settings => State.FromFeed(this, _userService.Settings);
}
