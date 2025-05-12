namespace Chefs.Services.Settings;

/// <summary>
/// Implements user settings related methods
/// </summary>
public interface ISettingsService
{
	///<summary>
	/// Gets chef settings
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// App settings from the phone
	/// </returns>
	ValueTask<AppConfig> GetSettings(CancellationToken ct);

	///<summary>
	/// Update app settings
	/// </summary>
	/// <param name="chefSettings">new settings</param>
	/// <param name="ct"></param>
	/// <returns>
	/// </returns>
	Task SetSettings(AppConfig chefSettings, CancellationToken ct);

	///<summary>
	/// Update app settings with specified properties without overwriting the rest
	/// </summary>
	/// <param name="ct"></param>
	/// <param name="title">App title</param>
	/// <param name="isDark">App theme flag</param>
	/// <param name="notification">User notifications flag</param>
	/// <param name="accentColor">Accent color</param>
	/// <returns>
	/// </returns>
	Task UpdateSettings(CancellationToken ct, string? title = null, bool? isDark = null, bool? notification = null, string? accentColor = null);
}
