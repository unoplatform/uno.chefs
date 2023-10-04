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

	//public IFeed<Mapsui.Map> Map => Feed.Async<Mapsui.Map>(CreateMap);

	public IFeed<Mapsui.Map> CurrentMap => Feed.Async(_mapService.GetCurrentMapAsync);

	public MapModel(INavigator navigator, IUserService userService, IMapService mapService)
	{
		_navigator = navigator;
		_userService = userService;
		_mapService = mapService;
	}

	//private async Task<Mapsui.Map> CreateMap()
	//{
	//	var map = new Mapsui.Map();

	//	map.Layers.Add(OpenStreetMap.CreateTileLayer());
	//	map.Layers.Add(CreatePointLayer());
	//	map.Home = n => n.CenterOnAndZoomTo(map.Layers[1].Extent!.Centroid, n.Resolutions[5]);
	//	return map;
	//}

	//private Layer CreatePointLayer()
	//{
	//	return new Layer
	//	{
	//		Name = "Point",
	//		DataSource = new MemoryProvider(GetContributors()),
	//		IsMapInfoLayer = true
	//	};
	//}

	//private async IAsyncEnumerable<IEnumerable<IFeature>> GetContributors()
	//{
	//	//var contributors = new List<Contributor> 
	//	//{ 
	//	//	//create mock data
	//	//	new Contributor { Name = "John Doe", Lat = 45.4976, Lng = -73.5701 },
	//	//	new Contributor { Name = "Noemi Macavei", Lat = 45.4985, Lng = -73.5697 },
	//	//	new Contributor { Name = "Mathias Huysmans", Lat = 45.5007, Lng = -73.5668 }
	//	//};

	//	var contributors = await PopularCreators;

	//	yield return contributors.Select(c =>
	//	{
	//		var feature = new PointFeature(SphericalMercator.FromLonLat(c.Location.Lng ?? 0, c.Location.Lat ?? 0).ToMPoint());
	//		feature[nameof(User.FullName)] = c.FullName;
	//		feature[nameof(User.Location.Lat)] = c.Location.Lat;
	//		feature[nameof(User.Location.Lng)] = c.Location.Lng;
	//		feature.Styles.Add(CreateCalloutStyle(feature.ToStringOfKeyValuePairs()));
	//		return (IFeature)feature;
	//	});
	//}

	private async Task<IImmutableList<User>> GetPopularCreatorsAsync()
	{
		return await _userService.GetPopularCreators(CancellationToken.None);
	}

	//private class Contributor
	//{
	//	public string? Name { get; set; }
	//	public double Lat { get; set; }
	//	public double Lng { get; set; }
	//}

	private static CalloutStyle CreateCalloutStyle(string content)
	{
		return new CalloutStyle
		{
			Title = content,
			TitleFont = { FontFamily = null, Size = 12, Italic = false, Bold = true },
			TitleFontColor = Color.Gray,
			MaxWidth = 120,
			RectRadius = 10,
			ShadowWidth = 4,
			Enabled = false,
			SymbolOffset = new Offset(0, SymbolStyle.DefaultHeight * 1f)
		};
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