using Chefs.Presentation.Messages;

namespace Chefs.Views.Flyouts;

public partial class ResponsiveDrawerFlyout : Flyout, IRecipient<ThemeChangedMessage>
{
	private const int WideBreakpoint = 800;
	private const int WidestBreakpoint = 1080;

	private FlyoutPresenter? _presenter;

	public ResponsiveDrawerFlyout()
	{
		this.InitializeComponent();
		WeakReferenceMessenger.Default.Register(this);
	}

	private void OnOpening(object? sender, object e)
	{
		if (_presenter is { } presenter)
		{
			var width = XamlRoot?.Size.Width ?? 0;
			if (width >= WideBreakpoint)
			{
				var gridLength = width > WidestBreakpoint ? 0.33 : 0.66;

				DrawerFlyoutPresenter.SetDrawerLength(presenter, new GridLength(gridLength, GridUnitType.Star));
				DrawerFlyoutPresenter.SetOpenDirection(presenter, DrawerOpenDirection.Left);
				DrawerFlyoutPresenter.SetIsGestureEnabled(presenter, false);
				presenter.CornerRadius = new CornerRadius(20, 0, 0, 20);
			}
			else
			{
				DrawerFlyoutPresenter.SetDrawerLength(presenter, new GridLength(1, GridUnitType.Star));
				DrawerFlyoutPresenter.SetIsGestureEnabled(presenter, false);
			}

			// Workaround for https://github.com/unoplatform/uno.chefs/issues/1436
			// Not explicitly setting thickness causes thickness to be set to a value greater than 1 sometime during runtime
#if __IOS__
			presenter.BorderThickness = new Thickness(0);
#endif
		}
	}

	protected override Control? CreatePresenter()
	{
		var basePresenter = base.CreatePresenter();

		_presenter = basePresenter as FlyoutPresenter;

		return basePresenter;
	}

	void IRecipient<ThemeChangedMessage>.Receive(ThemeChangedMessage message)
	{
		// Workaround for https://github.com/unoplatform/uno.chefs/issues/1017
#if WINDOWS
		_ = DispatcherQueue.TryEnqueue(() =>
		{
			MainLayout.RequestedTheme = message.IsDark ? ElementTheme.Dark : ElementTheme.Light;
		});
#endif
	}
}
