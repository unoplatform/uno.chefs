namespace Chefs.Views.Flyouts;

public abstract partial class ResponsiveDrawerFlyout : Flyout
{
	protected abstract DrawerOpenDirection WideOpenDirection { get; }

	//private FlyoutPresenter? _presenter;

	public ResponsiveDrawerFlyout()
	{
		this.InitializeComponent();
	}
}
