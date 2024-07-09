namespace Chefs.Presentation;

public partial class ProfileModel
{
	private readonly IRecipeService _recipeService;
	private readonly INavigator _navigator;

	public ProfileModel(
		INavigator navigator,
		IRecipeService recipeService,
		IUserService userService,
		User? user)
	{
		_navigator = navigator;
		_recipeService = recipeService;

		Profile = user != null ? State.Value(this, () => user) : userService.User;
	}

	public IFeed<User> Profile { get; }

	public IListFeed<Recipe> Recipes => Profile
		.SelectAsync((user, ct) => _recipeService.GetByUser(user.Id, ct))
		.AsListFeed();
}
