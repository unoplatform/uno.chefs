using Uno.Extensions.Toolkit;
using Windows.Management.Deployment;
using AppTheme = Uno.Extensions.Toolkit.AppTheme;

namespace Chefs.Presentation;

public partial class SettingsModel
{
	private readonly IUserService _userService;
	private readonly IThemeService _themeService;
	private readonly User _user;

	public SettingsModel(
		IThemeService themeService,
		IUserService userService,
		User user)
	{
		_userService = userService;
		_themeService = themeService;
		_user = user;

		Settings.ForEachAsync(async (settings, ct) =>
		{
			if (settings is { })
			{
				await _themeService.SetThemeAsync((settings.IsDark ?? false) ? AppTheme.Dark : AppTheme.Light);
				await _userService.SetSettings(settings, ct);
			}
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

	public IState<User> Profile => State.Value(this, () => _user);

	public IState<AppConfig> Settings => State.Async(this, async ct => await _userService.GetSettings(ct));
}
