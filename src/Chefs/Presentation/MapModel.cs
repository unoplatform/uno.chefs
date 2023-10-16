namespace Chefs.Presentation;

public partial class MapModel
{
	private readonly INavigator _navigator;
	private readonly IUserService _userService;

	public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);

	public IFeed<User> UserProfile => _userService.UserFeed;

	public MapModel(INavigator navigator, IUserService userService)
	{
		_navigator = navigator;
		_userService = userService;
	}
}