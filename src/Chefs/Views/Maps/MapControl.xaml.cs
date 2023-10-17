using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.Utilities;
using Microsoft.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Chefs.Views.Maps
{
	public sealed partial class MapControl : UserControl
	{
		private static Mapsui.Map? _map;
		private static MyLocationLayer? _myLocationLayer;

		public ICollectionView Users
		{
			get { return (ICollectionView)GetValue(UsersProperty); }
			set { SetValue(UsersProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Users.  This enables animation, styling, binding, etc.
		public static readonly DependencyProperty UsersProperty =
			DependencyProperty.Register("Users", typeof(ICollectionView), typeof(MapControl), new PropertyMetadata(0, new PropertyChangedCallback(OnUsersChanged)));

		private static void OnUsersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			_map = ((MapControl)d).ContributorsMap.Map;
			// We can't use the generic type because mvu-x hides it from us
			if (e.NewValue is not ICollectionView contributors)
			{
				return;
			}

			AddPinsLayer(contributors);
			AddMyLocationLayer();

			_map.Info += (s, e) =>
			{
				Console.WriteLine(e.MapInfo);
			};
		}

		private static void AddMyLocationLayer()
		{
			// TODO: Get real location
			var startingPosition = _map!.Layers[1].Extent!.Centroid;

			_myLocationLayer = new MyLocationLayer(_map!);
			_myLocationLayer.UpdateMyLocation(startingPosition);

			_map!.Layers.Add(_myLocationLayer);
			CenterOnPoint(startingPosition, 12);
		}

		private static void AddPinsLayer(ICollectionView contributors)
		{
			var pins = contributors.OfType<User>().Select(user =>
			{
				var feature = new PointFeature(SphericalMercator.FromLonLat(user.Location.Lng ?? 0, user.Location.Lat ?? 0).ToMPoint());
				feature[nameof(User.FullName)] = user.FullName;
				feature[nameof(User.Location.Lat)] = user.Location.Lat;
				feature[nameof(User.Location.Lng)] = user.Location.Lng;
				feature[nameof(User.Id)] = user.Id;
				feature.Styles.Add(CreateCalloutStyle(feature.ToStringOfKeyValuePairs()));
				return (IFeature)feature;
			});

			var pinsLayer = new MemoryLayer
			{
				Name = "Contributor pins with callouts",
				IsMapInfoLayer = true,
				Features = new MemoryProvider(pins).Features,
				Style = new SymbolStyle()
				{
					BitmapId = typeof(MapControl).LoadSvgId(@"Assets.Maps.location_pin.svg"),
					SymbolScale = 1,
					SymbolOffset = new Offset(x: 0.0, y: 0.5, isRelative: true)
				}
			};

			_map!.Layers.Add(pinsLayer);
		}

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

		private static void CenterOnPoint(MPoint point, int resolution)
		{
			_map!.Home = navigator => navigator.CenterOnAndZoomTo(point, navigator.Resolutions[resolution]);
		}

		public MapControl()
		{
			this.InitializeComponent();

			ContributorsMap.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
			ContributorsMap.Map.Widgets.Add(new Mapsui.Widgets.Zoom.ZoomInOutWidget { MarginX = 10, MarginY = 20 });
		}
	}
}
