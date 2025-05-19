using System.Collections.Immutable;
using Android.Runtime;
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
	static Application()
	{
		App.InitializeLogging();
	}

	public Application(IntPtr javaReference, JniHandleOwnership transfer)
		: base(() => new App(), javaReference, transfer)
	{
	}
}
