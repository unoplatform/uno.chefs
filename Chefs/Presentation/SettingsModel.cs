using Chefs.Presentation.Messages;

namespace Chefs.Presentation;

public partial record SettingsModel
{
	private readonly IUserService _userService;
	private readonly IThemeService _themeService;
	private readonly ISettings _settings;
	private readonly IMessenger _messenger;
	private readonly User _user;

	public SettingsModel(
		IThemeService themeService,
		IUserService userService,
		ISettings settings,
		IMessenger messenger,
		User user)
	{
		_userService = userService;
		_themeService = themeService;
		_settings = settings;
		_user = user;
		_messenger = messenger;

		_themeService.ThemeChanged += OnThemeChanged;
	}

	private void OnThemeChanged(object? sender, AppTheme theme) => _messenger.Send(new ThemeChangedMessage(theme));

	public IList<AppTheme> ThemeOptions => Enum.GetValues(typeof(AppTheme)).Cast<AppTheme>().ToList();

	public IState<AppTheme> Theme => State
		.Value(this, () => _themeService.Theme)
		.ForEach(async (theme, _) => await _themeService.SetThemeAsync(theme));

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
	public IState<bool> NotificationsEnabled => State
		.Value(this, () => bool.TryParse(_settings.Get("NotificationsEnabled"), out var enabled) && enabled)
		.ForEach(async (enabled, ct) => _settings.Set("NotificationsEnabled", enabled.ToString()));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

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
