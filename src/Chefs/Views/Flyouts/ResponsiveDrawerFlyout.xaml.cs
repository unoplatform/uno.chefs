using Microsoft.UI.Xaml.Media;
using Uno.UI;
using Uno.UI.Extensions;

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
		if (_presenter is { } presenter 
			&& sender is Flyout flyout 
			&& flyout.XamlRoot is { } root
			)
		{
			//// TODO: Use Responsive Helpers?
			if (root.Size.Width > 700)
			{
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
