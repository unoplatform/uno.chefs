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
		
		// Initialize theme from persisted settings on a background thread
		// to avoid potential race conditions with reactive state initialization
		_ = InitializeThemeAsync();
	}

	private async Task InitializeThemeAsync()
	{
		var savedTheme = Enum.TryParse<AppTheme>(_settings.Get("Theme"), out var theme) ? theme : AppTheme.System;
		await _themeService.SetThemeAsync(savedTheme);
	}

	private void OnThemeChanged(object? sender, AppTheme theme) => _messenger.Send(new ThemeChangedMessage(theme));

	public IListFeed<AppTheme> ThemeOptions => State
		.Value(this, () => Enum.GetValues(typeof(AppTheme)).Cast<AppTheme>().ToImmutableList())
		.AsListFeed()
		.Selection(Theme);

	public IState<AppTheme> Theme => State
		.Value(this, () => Enum.TryParse<AppTheme>(_settings.Get("Theme"), out var theme) ? theme : AppTheme.System)
		.ForEach(async (theme, ct) => 
		{
			_settings.Set("Theme", theme.ToString());
			await _themeService.SetThemeAsync(theme);
		});

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
