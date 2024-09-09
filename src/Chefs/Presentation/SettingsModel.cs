using Chefs.Presentation.Messages;
using AppTheme = Uno.Extensions.Toolkit.AppTheme;

namespace Chefs.Presentation;

public partial record SettingsModel
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
	}

	public IState<AppConfig> Settings => State
		.Async(this, _userService.GetSettings)
		.ForEach(async (settings, ct) =>
		{
			if (settings is { })
			{
				var isDark = (settings.IsDark ?? false);
				await _themeService.SetThemeAsync(isDark ? Uno.Extensions.Toolkit.AppTheme.Dark : Uno.Extensions.Toolkit.AppTheme.Light);
				await _userService.SetSettings(settings, ct);

				WeakReferenceMessenger.Default.Send(new ThemeChangedMessage(isDark));
			}
		});
	public IState<User> Profile => State
		.Value(this, () => _user)
		.ForEach(async (profile, ct) =>
		{
			if (profile is null)
			{
				return;
			}

			await _userService.Update(profile, ct);
		});
}
