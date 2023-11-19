using Android.Runtime;
using Com.Nostra13.Universalimageloader.Core;
using Microsoft.UI.Xaml.Media;
using System.Collections.Immutable;

namespace Chefs.Droid
{
	[global::Android.App.ApplicationAttribute(
		Label = "@string/ApplicationName",
		Icon = "@mipmap/iconapp",
		LargeHeap = true,
		HardwareAccelerated = true,
		Theme = "@style/AppTheme"
	)]
	public class Application : Microsoft.UI.Xaml.NativeApplication
	{
		public Application(IntPtr javaReference, JniHandleOwnership transfer)
			: base(() => new AppHead(), javaReference, transfer)
		{
			ConfigureUniversalImageLoader();

			// LinkDescription commented for now in the Chefs.Mobile.csproj as it is failing the Android Build
			// Workaround in place here for System.Collections.Immutable.ImmutableList
			var unused = ImmutableList.Create<string>();
			var unused2 = ImmutableList.CreateBuilder<string>();
			var unused3 = ImmutableList.CreateRange<string>(new string[] { "a", "b" });
			var unused4 = ImmutableList.Create("a");
			var unused5 = ImmutableList.Create(new string[] { "a", "b" });
		}

		private static void ConfigureUniversalImageLoader()
		{
			// Create global configuration and initialize ImageLoader with this config
			ImageLoaderConfiguration config = new ImageLoaderConfiguration
				.Builder(Context)
				.Build();

			ImageLoader.Instance.Init(config);

			ImageSource.DefaultImageLoader = ImageLoader.Instance.LoadImageAsync;
		}
	}
}
