using Uno.Extensions.Toolkit;
using Windows.Management.Deployment;
using AppTheme = Uno.Extensions.Toolkit.AppTheme;

namespace Chefs.Presentation;

public partial class SettingsModel
{
	private readonly IUserService _userService;
	private readonly IThemeService _themeService;

	public SettingsModel(
		IThemeService themeService,
		IUserService userService,
		User user)
	{
		_userService = userService;
		_themeService = themeService;

		Profile = State.Value(this, () => user);
		Settings = State.Async(this, async ct =>
		{
			var settings = await _userService.Settings;
			return new AppConfig
			{
				IsDark = _themeService.IsDark,
				Notification = settings?.Notification,
			};
		});

		Settings.ForEachAsync(async (settings, ct) =>
		{
			await _themeService.SetThemeAsync((settings?.IsDark ?? false) ? AppTheme.Dark : AppTheme.Light);
			await _userService.Settings.UpdateAsync(_ => settings, ct);
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

	public IState<AppConfig> Settings { get; }
}
