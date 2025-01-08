namespace Chefs.Services.Users;

/// <summary>
/// Implements user related methods
/// </summary>
public interface IUserService
{
	/// <summary>
	/// Current user data
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// User logged in
	/// </returns>
	ValueTask<User> GetCurrent(CancellationToken ct);

	/// <summary>
	/// Feed of the current user.
	/// </summary>
	IFeed<User> User { get; }

	/// <summary>
	/// Update user information
	/// </summary>
	/// <param name="user">user with information to update</param>
	/// <param name="ct"></param>
	/// <returns>
	/// </returns>
	ValueTask Update(User user, CancellationToken ct);

	///<summary>
	/// Gets chef settings
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// App settings from the phone
	/// </returns>
	ValueTask<AppConfig> GetSettings(CancellationToken ct);

	/// <summary>
	/// Porpular creators related with the recipes
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Return users that they are popular by their recipes
	/// </returns>
	ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct);

	/// <summary>
	/// Returns specific user
	/// </summary>
	/// <param name="userId">User GUID</param>
	/// <returns>
	/// User
	/// </returns>
	ValueTask<User> GetById(Guid userId, CancellationToken ct);

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

	// <summary>
	// Authentication method
	// </summary>
	// <param name="email"> The user email </param>
	// <param name="ct"></param>
	// <returns>
	// User logged in
	// </returns>
	// In case we need auth
	//ValueTask<bool> BasicAuthenticate(string email, string password, CancellationToken ct);
}
