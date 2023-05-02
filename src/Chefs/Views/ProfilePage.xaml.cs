namespace Chefs.Views;

public sealed partial class ProfilePage : ResponsiveDrawerFlyout
{
	public ProfilePage()
	{
		this.InitializeComponent();

		Opening += ApplyLeftDrawerFlyoutPresenterStyle;
	}
}
