using Microsoft.UI.Xaml.Media;

namespace Chefs.Views;

public sealed partial class ShellControl : UserControl, IContentControlProvider
{
	public ExtendedSplashScreen SplashScreen => Splash;
	public ShellControl()
	{
		this.InitializeComponent();

	}

	public ContentControl ContentControl => Splash;

	public Frame? RootFrame => FindFirstDescendant<Frame>(Splash);

	// Uno Platform's FindFirstDescendant extension is no longer public on WinAppSDK.
	private static T? FindFirstDescendant<T>(DependencyObject parent) where T : DependencyObject
	{
		var count = VisualTreeHelper.GetChildrenCount(parent);
		for (var i = 0; i < count; i++)
		{
			var child = VisualTreeHelper.GetChild(parent, i);
			if (child is T match)
			{
				return match;
			}

			if (FindFirstDescendant<T>(child) is { } descendant)
			{
				return descendant;
			}
		}

		return null;
	}
}
