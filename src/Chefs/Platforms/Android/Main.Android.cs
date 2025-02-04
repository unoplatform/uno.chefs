using System.Collections.Immutable;
using Android.Runtime;
#if !IS_ANDROID_SKIA
using Com.Nostra13.Universalimageloader.Core;
#endif
using Microsoft.UI.Xaml.Media;

namespace Chefs.Droid;

[global::Android.App.ApplicationAttribute(
	Label = "@string/ApplicationName",
	Icon = "@mipmap/icon",
	LargeHeap = true,
	HardwareAccelerated = true,
	Theme = "@style/AppTheme"
)]
public class Application : Microsoft.UI.Xaml.NativeApplication
{
	public Application(IntPtr javaReference, JniHandleOwnership transfer)
		: base(() => new App(), javaReference, transfer)
	{
#if !IS_ANDROID_SKIA
		ConfigureUniversalImageLoader();
#endif
	}

#if !IS_ANDROID_SKIA
	private static void ConfigureUniversalImageLoader()
	{
		// Create global configuration and initialize ImageLoader with this config
		ImageLoaderConfiguration config = new ImageLoaderConfiguration
			.Builder(Context)
			.Build();

		ImageLoader.Instance.Init(config);

		ImageSource.DefaultImageLoader = ImageLoader.Instance.LoadImageAsync;
	}
#endif
}
