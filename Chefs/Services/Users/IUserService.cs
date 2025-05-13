namespace Chefs.Services.Users;

/// <summary>
/// Provides methods for managing user data and retrieving user-related information.
/// </summary>
public interface IUserService
{
	/// <summary>
	/// Gets the currently logged-in user.
	/// </summary>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>The current <see cref="User"/>.</returns>
	ValueTask<User> GetCurrent(CancellationToken ct);

	/// <summary>
	/// Gets a feed of the current user.
	/// </summary>
	IFeed<User> User { get; }

	/// <summary>
	/// Updates the specified user's information.
	/// </summary>
	/// <param name="user">The user with updated information.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask Update(User user, CancellationToken ct);

	/// <summary>
	/// Retrieves a list of popular creators based on their recipes.
	/// </summary>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>A list of users who are popular for their recipes.</returns>
	ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct);

	/// <summary>
	/// Gets a user by their unique identifier.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>The <see cref="User"/> with the specified ID.</returns>
	ValueTask<User> GetById(Guid userId, CancellationToken ct);

	// <summary>
	// Authenticates a user using email and password.
	// </summary>
	// <param name="email">The user's email address.</param>
	// <param name="password">The user's password.</param>
	// <param name="ct">A cancellation token.</param>
	// <returns>True if authentication is successful; otherwise, false.</returns>
	//ValueTask<bool> BasicAuthenticate(string email, string password, CancellationToken ct);
}
