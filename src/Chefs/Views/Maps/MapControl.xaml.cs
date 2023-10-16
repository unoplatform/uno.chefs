using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Microsoft.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Chefs.Views.Maps
{
	public sealed partial class MapControl : UserControl
	{
		private static Mapsui.Map? _map;
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
			CenterOnPoint(_map.Layers[1].Extent!.Centroid);
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
				Name = "Contributors with callouts",
				IsMapInfoLayer = true,
				Features = pins,
				Style = SymbolStyles.CreatePinStyle(pinColor: new Color(237, 63, 100), symbolScale: 0.7),
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

		private static void CenterOnPoint(MPoint point)
		{
			_map!.Home = navigator => navigator.CenterOnAndZoomTo(point, navigator.Resolutions[12]);
		}

		public MapControl()
		{
			this.InitializeComponent();

			ContributorsMap.Map.Layers.Add(OpenStreetMap.CreateTileLayer());

			ContributorsMap.Map.Widgets.Add(new Mapsui.Widgets.Zoom.ZoomInOutWidget { MarginX = 10, MarginY = 20 });
		}
	}
}
