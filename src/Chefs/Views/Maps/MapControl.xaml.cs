using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Microsoft.UI.Xaml.Data;
using System.Collections.Specialized;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Chefs.Views.Maps
{
    public sealed partial class MapControl : UserControl
    {
        public ICollectionView Users
        {
            get { return (ICollectionView)GetValue(UsersProperty); }
            set { SetValue(UsersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Users.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsersProperty =
            DependencyProperty.Register("Users", typeof(ICollectionView), typeof(MapControl), new PropertyMetadata(0, new PropertyChangedCallback(OnUsersChanged)));

        private static void OnUsersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = (MapControl)d;
            // We can't use the generic type because mvu-x hides it from us
            if (e.NewValue is not ICollectionView contributors)
            {
                return;
            }

            var pins = contributors.OfType<User>().Select(c =>
            {
                var feature = new PointFeature(SphericalMercator.FromLonLat(c.Location.Lng ?? 0, c.Location.Lat ?? 0).ToMPoint());
                feature[nameof(User.FullName)] = c.FullName;
                feature[nameof(User.Location.Lat)] = c.Location.Lat;
                feature[nameof(User.Location.Lng)] = c.Location.Lng;
                feature[nameof(User.Id)] = c.Id;
                feature.Styles.Add(CreateCalloutStyle(feature.ToStringOfKeyValuePairs()));
                return (IFeature)feature;
            });

            var memLayer = new MemoryLayer
            {
                Name = "Cities with callouts",
                IsMapInfoLayer = true,
                Features = pins,
                Style = SymbolStyles.CreatePinStyle(symbolScale: 0.7),
            };

            mapControl.Map.Map.Layers.Add(memLayer);
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
        public MapControl()
        {
            this.InitializeComponent();

            Map.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        }
    }
}
