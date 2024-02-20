---
uid: Mapsui
---

# How to set up an interactive map

## Problem

Applications often need to show an interactive map. Displaying a map is a complex operation.

## Solution

Mapsui simplify the implementation for multi-platform map projects. This example is showing a map, add pins, add user location and an info layer to pins. Mapsui is an external mapping component.

<img src="../assets/Mapsui.gif" height="500px" alt="MapControl"/>

### Adding Mapsui to your page

Start by adding the map element in your page.

```xml
<Page x:Class="Chefs.Views.MapPage"
    xmlns:map="using:Mapsui.UI.WinUI"
>
...
<map:MapControl x:Name="MapControl"
				utu:AutoLayout.PrimaryAlignment="Stretch" />
```

### Setting up the map page

In the code-behind file, import Mapsui.

```csharp
// MapPage.xaml.cs.
using BruTile.Predefined;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using Mapsui.Utilities;
using Mapsui.Widgets.Zoom;
```

Instantiate a map object.

```csharp
private static Mapsui.Map? _map;
```

### Initializing the Map

Add initialization to your constructor.

```csharp
	public MapPage()
	{
		this.InitializeComponent();

#if HAS_UNO
		// All Platforms except Windows.
        this.Loaded += (sender, e) => InitializeMap();
#else
        // Windows.
		InitializeMap();
#endif
	}
```

Add the `InitializeMap` method.

```csharp
private void InitializeMap()
{
	// "MapControl" should match the name of your control on MapPage.xaml.
	_map = MapControl.Map;
	// Add the map.
	AddBaseLayer();
	// Add pins on the map.
	AddPinsLayer();
	// Show Location of the user.
	AddMyLocationLayer();
	// Trigger info bubble event.
	_map.Info += MapOnInfo;
}
```

### Base map layer.

```csharp
private static void AddBaseLayer()
{
    //KnownTileSource is provided by Brutile.
	_map!.Layers.Add(layers: new TileLayer(KnownTileSources.Create(KnownTileSource.BingRoads)));
	_map!.Widgets.Add(new ZoomInOutWidget { MarginX = 36, MarginY = 36 });
}
```

### Pins Layer

```csharp
// Hardcoded contributors.
private static List<Contributor> Contributors => new List<Contributor>
{
	new Contributor("Troyan Smith", 45.5018, -73.5566, 45),
	new Contributor("Niki Samantha", 45.5411, -73.5770, 20),
	new Contributor("Mike Baker", 45.5113, -73.5961, 15)
};

private record Contributor(string? Name, double Lat, double Lng, int Recipes);

// This defines the style of the info bubble.
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

private static void AddPinsLayer()
{
	var pins = Contributors.Select(c =>
	{
		// Set the pin with a location.
		var feature = new PointFeature(SphericalMercator.FromLonLat(c.Lng, c.Lat).ToMPoint());
		// Info bubble is added to each pin.
		feature.Styles.Add(CreateCalloutStyle(title: c.Name!, subtitle: $"{c.Recipes} recipes"));
		return (IFeature)feature;
	});

	//Create a layer containing Pins.
	var pinsLayer = new MemoryLayer
	{
		Name = "Contributor pins with callouts",
		// Enable this layer to be clickable.
		IsMapInfoLayer = true,
		Features = new MemoryProvider(pins).Features,
		// Set the pin style.
		Style = new SymbolStyle()
		{
			BitmapId = typeof(MapPage).LoadSvgId(@"Assets.Maps.location_pin.svg"),
			SymbolScale = 1,
			SymbolOffset = new Offset(x: 0.0, y: 0.5, isRelative: true)
		}
	};

	// Add the pin layer to the map.
	_map!.Layers.Add(pinsLayer);
}

```

### Location layer

```csharp
private static void AddMyLocationLayer()
{
	// TODO: Get the user's real location. For now, the starting position is calculated as the centroid of all pins.
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

	// Set the camera position at the user's location.
	CenterOnPoint(startingPosition, 13);
}

private static void CenterOnPoint(MPoint point, int resolution)
{
	_map!.Home = navigator => navigator.CenterOnAndZoomTo(point, navigator.Resolutions[resolution]);
}
```

### Triggering the Map Info Bubble.

```csharp
private static void MapOnInfo(object? sender, MapInfoEventArgs e)
{
	// Get the pin if the event is on a pin.
	var calloutStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();
	if (calloutStyle != null)
	{
		calloutStyle.Enabled = !calloutStyle.Enabled;
		// Draw the new layer.
		e.MapInfo?.Layer?.DataHasChanged();
	}
}
```

## Source Code

Chefs app

- [Map.xaml.cs](https://github.com/unoplatform/uno.chefs/blob/e02a4dce407e13b933d2e8e6c764d237ebc11d33/src/Chefs/Views/MapPage.xaml.cs#L14)
- [Map.xaml](https://github.com/unoplatform/uno.chefs/blob/e02a4dce407e13b933d2e8e6c764d237ebc11d33/src/Chefs/Views/MapPage.xaml#L32)


## Documentation

- [Mapsui documentation](https://mapsui.com/documentation/getting-started-uno-winui.html)