namespace Chefs.Views;

public sealed partial class NotificationsPage : ResponsiveDrawerFlyout
{
	public NotificationsPage()
	{
		this.InitializeComponent();

		Opening += ApplyLeftDrawerFlyoutPresenterStyle;
	}
}
