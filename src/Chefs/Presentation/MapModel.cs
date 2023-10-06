using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.WinUI;

namespace Chefs.Presentation;

public partial class MapModel
{
	private readonly INavigator _navigator;
	private readonly IUserService _userService;
	private readonly IMapService _mapService;

	public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);

	public IFeed<User> UserProfile => _userService.UserFeed;

	public IFeed<Mapsui.Map> CurrentMap => Feed.Async(_mapService.GetCurrentMapAsync);

	public MapModel(INavigator navigator, IUserService userService, IMapService mapService)
	{
		_navigator = navigator;
		_userService = userService;
		_mapService = mapService;
	}

	public async Task ShowProfile(User profile)
	{
		await NavigateToProfile(profile);
	}

	public async Task ShowCurrentProfile()
	{
		await NavigateToProfile();
	}

	private async Task NavigateToProfile(User? profile = null)
	{
		var response = await _navigator.NavigateRouteForResultAsync<IChefEntity>(this, "Profile", data: profile);
		var result = await response!.Result;

		await (result.SomeOrDefault() switch
		{
			UpdateCookbook updateCookbook => _navigator.NavigateViewModelAsync<CreateUpdateCookbookModel>(this, data: updateCookbook.Cookbook),
			Cookbook cookbook when cookbook.Id == Guid.Empty => _navigator.NavigateViewModelAsync<CreateUpdateCookbookModel>(this),
			Cookbook cookbook => _navigator.NavigateViewModelAsync<CookbookDetailModel>(this, data: cookbook),
			object obj when obj is not null && obj.GetType() != typeof(object) => _navigator.NavigateDataAsync(this, obj),
			_ => Task.CompletedTask,
		});
	}
}