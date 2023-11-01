using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.Utilities;
using Mapsui.Widgets.Zoom;

namespace Chefs.Views;

public sealed partial class MapPage : Page
{
	private static Mapsui.Map? _map;
	private static MyLocationLayer? _myLocationLayer;

	private static List<Contributor> Contributors => new List<Contributor>
	{
		new Contributor("Troyan Smith", 45.5018, -73.5566, 45),
		new Contributor("Niki Samantha", 45.5411, -73.5770, 20),
		new Contributor("Mike Baker", 45.5113, -73.5961, 15)
	};

	private record Contributor(string? Name, double Lat, double Lng, int Recipes);
	
	public MapPage()
	{
		this.InitializeComponent();

#if HAS_UNO
		this.Loaded += (sender, e) => InitializeMap();
#else
		InitializeMap();
#endif
	}

	private void InitializeMap()
	{
		_map = MapControl.Map;
		AddBaseLayer();
		AddPinsLayer();
		AddMyLocationLayer();
		_map.Info += MapOnInfo;
	}

	private static void AddBaseLayer()
	{
		_map!.Layers.Add(OpenStreetMap.CreateTileLayer());
		// Add "using Mapsui.Tiling.Layers;" and uncomment following line to use a different tile source
		//_map!.Layers.Add(layers: new TileLayer(KnownTileSources.Create(KnownTileSource.BingAerial)));

		_map!.Widgets.Add(new ZoomInOutWidget { MarginX = 36, MarginY = 36 });
	}

	private static void AddPinsLayer()
	{
		var pins = Contributors.Select(c =>
		{
			var feature = new PointFeature(SphericalMercator.FromLonLat(c.Lng, c.Lat).ToMPoint());
			feature.Styles.Add(CreateCalloutStyle(title: c.Name!, subtitle: $"{c.Recipes} recipes"));
			return (IFeature)feature;
		});

		var pinsLayer = new MemoryLayer
		{
			Name = "Contributor pins with callouts",
			IsMapInfoLayer = true,
			Features = new MemoryProvider(pins).Features,
			Style = new SymbolStyle()
			{
				BitmapId = typeof(MapPage).LoadSvgId(@"Assets.Maps.location_pin.svg"),
				SymbolScale = 1,
				SymbolOffset = new Offset(x: 0.0, y: 0.5, isRelative: true)
			}
		};

		_map!.Layers.Add(pinsLayer);
	}

	private static CalloutStyle CreateCalloutStyle(string title, string subtitle)
	{
		return new CalloutStyle
		{
			Title = title,
			TitleFont = { Size = 14 },
			TitleFontColor = Color.Black,
			Subtitle = subtitle,
			SubtitleFont = { Size = 12 },
			SubtitleFontColor = Color.FromArgb(97, 28, 27, 31),
			Type = CalloutType.Detail,
			MaxWidth = 120,
			RectRadius = 10,
			Enabled = false,
			SymbolOffset = new Offset(0, SymbolStyle.DefaultHeight * 1f)
		};
	}

	private static void AddMyLocationLayer()
	{
		// TODO: Get real location
		var startingPosition = _map!.Layers[1].Extent!.Centroid;

		_myLocationLayer = new MyLocationLayer(_map!)
		{
			CalloutText = "My location",
			Style = new SymbolStyle
			{
				BitmapId = typeof(MapPage).LoadSvgId(@"Assets.Maps.location_circle.svg"),
				SymbolScale = 1
			}
		};
		
		_myLocationLayer.UpdateMyLocation(startingPosition);
		_map!.Layers.Add(_myLocationLayer);
		CenterOnPoint(startingPosition, 13);
	}

	private static void CenterOnPoint(MPoint point, int resolution)
	{
		_map!.Home = navigator => navigator.CenterOnAndZoomTo(point, navigator.Resolutions[resolution]);
	}

	private static void MapOnInfo(object? sender, MapInfoEventArgs e)
	{
		var calloutStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();
		if (calloutStyle != null)
		{
			calloutStyle.Enabled = !calloutStyle.Enabled;
			e.MapInfo?.Layer?.DataHasChanged();
		}
	}
}
