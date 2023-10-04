using Mapsui;
using Mapsui.Animations;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI;
using Mapsui.Widgets.Zoom;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

namespace Chefs.Views;

public sealed partial class MapPage : Page
{
	public MapPage()
	{
		this.InitializeComponent();

		//zoomSlider.ValueChanged += ZoomSlider_ValueChanged;

		//MapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
		//MyMap.Map.Widgets.Add(new ZoomInOutWidget { MarginX = 10, MarginY = 20 });
		//MyMap.PointerMoved += MyMap_PointerMoved;
		//var centerOfLondonOntario = new MPoint(-81.2497, 42.9837);

		//var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(centerOfLondonOntario.X, centerOfLondonOntario.Y).ToMPoint();

		//MyMap.Map.Home = n => n.CenterOnAndZoomTo(sphericalMercatorCoordinate, n.Resolutions[13]);
	}

	//private void MyMap_PointerMoved(object sender, PointerRoutedEventArgs e)
	//{
	//	_currentPoint = e.GetCurrentPoint(this);
	//}

	//private void ZoomSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
	//{
	//	if (_currentPoint is null)
	//	{
	//		return;
	//	}
	//	var mousePosition = new MPoint(_currentPoint.Position.X, _currentPoint.Position.Y);

	//	if (MyMap.Map == null)
	//	{
	//		return;
	//	}
	//	if (e.NewValue > e.OldValue)
	//	{
	//		MyMap.Map.Navigator?.ZoomIn(mousePosition, 100, MouseWheelAnimation.Easing);
	//	}
	//	else
	//	{
	//		MyMap.Map.Navigator?.ZoomOut(mousePosition, 100, MouseWheelAnimation.Easing);
	//	}
	//}

	//public MouseWheelAnimation MouseWheelAnimation { get; } = new MouseWheelAnimation { Duration = 0 };

	//private Microsoft.UI.Input.PointerPoint? _currentPoint;
}
