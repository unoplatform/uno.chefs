namespace Chefs.Services.Settings;

public class SettingsService(IWritableOptions<AppConfig> chefAppOptions) : ISettingsService
{
	public async ValueTask<AppConfig> GetSettings(CancellationToken ct)
		=> chefAppOptions.Value;

	public async Task SetSettings(AppConfig chefSettings, CancellationToken ct)
	{
		await chefAppOptions.UpdateAsync(_ => chefSettings);
	}

	public async Task UpdateSettings(CancellationToken ct, string? title = null, bool? isDark = null, bool? notification = null, string? accentColor = null)
	{
		var currentSettings = await GetSettings(ct);

		var newSettings = currentSettings with
		{
			Title = title ?? currentSettings.Title,
			IsDark = isDark ?? currentSettings.IsDark,
			Notification = notification ?? currentSettings.Notification,
			AccentColor = accentColor ?? currentSettings.AccentColor,
		};

		await SetSettings(newSettings, ct);
	}
}
