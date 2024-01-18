using Windows.Media.Core;

namespace Chefs.Views;

public sealed partial class LiveCookingPage : Page
{
	public LiveCookingPage()
	{
		this.InitializeComponent();

		mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Chefs/Assets/Videos/CookingVideo.mp4"));
	}
}
