namespace Chefs.Views.Flyouts;

public abstract partial class ResponsiveDrawerFlyout : Flyout
{
	protected abstract DrawerOpenDirection WideOpenDirection { get; }

	private readonly DrawerFlyoutPresenter _presenter;

	public ResponsiveDrawerFlyout()
	{
		this.InitializeComponent();
		
		_presenter = new DrawerFlyoutPresenter() 
		{ 
			OpenDirection = DrawerOpenDirection.Up,
			DrawerLength = new GridLength(1, GridUnitType.Star),
			Content = MainLayout 
		};
	}

	private void OnOpening(object? sender, object e)
	{
		if (sender is Flyout flyout && flyout.XamlRoot is { } root)
		{
			if (root.Size.Width > 700)
			{
				_presenter.OpenDirection = WideOpenDirection;
				_presenter.DrawerLength = new GridLength(0.66, GridUnitType.Star);
				MainLayout.CornerRadius = GetWideCornerRadius();
			}
			else
			{
				_presenter.IsGestureEnabled = false;
			}
		}
	}

		protected override Control CreatePresenter() => _presenter;

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
