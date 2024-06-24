namespace Chefs.Presentation;

public partial class ProfileModel
{
	private readonly IRecipeService _recipeService;
	public ProfileModel(
		IRecipeService recipeService,
		User user)
	{
		_recipeService = recipeService;
		
		Profile = State.Value(this, () => user);
	}

	public IState<User> Profile { get; }

	public IListFeed<Recipe> Recipes => Profile
		.SelectAsync((user, ct) => _recipeService.GetByUser(user.Id, ct))
		.AsListFeed();
}
