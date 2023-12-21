namespace Chefs.Views.Flyouts;

public abstract partial class ResponsiveDrawerFlyout : Flyout
{
	protected abstract DrawerOpenDirection WideOpenDirection { get; }

	private FlyoutPresenter? _presenter;

	public ResponsiveDrawerFlyout()
	{
		this.InitializeComponent();
	}

	private void OnOpening(object? sender, object e)
	{
		if (_presenter is { } presenter)
		{
			// TODO: Use Responsive markup extension in a custom style
			var responsiveHelper = ResponsiveHelper.GetForCurrentView();
			var width = responsiveHelper.WindowSize.Width;

			if (width >= responsiveHelper.Layout.Wide)
			{
				var gridLength = width > responsiveHelper.Layout.Widest ? 0.33 : 0.66;

				DrawerFlyoutPresenter.SetDrawerLength(presenter, new GridLength(gridLength, GridUnitType.Star));
				DrawerFlyoutPresenter.SetOpenDirection(presenter, WideOpenDirection);
				presenter.CornerRadius = GetWideCornerRadius();
			}
			else
			{
				DrawerFlyoutPresenter.SetDrawerLength(presenter, new GridLength(1, GridUnitType.Star));
				DrawerFlyoutPresenter.SetIsGestureEnabled(presenter, false);
			}
		}
	}

	protected override Control CreatePresenter()
	{
		var basePresenter = base.CreatePresenter();

		_presenter = basePresenter as FlyoutPresenter;

		return basePresenter;
	}

	private CornerRadius GetWideCornerRadius()
	{
		return WideOpenDirection switch
		{
			DrawerOpenDirection.Left => new CornerRadius(20, 0, 0, 20),
			DrawerOpenDirection.Right => new CornerRadius(0, 20, 20, 0),
			DrawerOpenDirection.Up => new CornerRadius(20, 20, 0, 0),
			DrawerOpenDirection.Down => new CornerRadius(0, 0, 20, 20),
			_ => throw new InvalidOperationException($"Invalid WideOpenDirection"),
		};
	}
}
