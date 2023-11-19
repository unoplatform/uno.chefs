namespace Chefs.Views;

public sealed partial class ReviewsPage : ResponsiveDrawerFlyout
{
	public ReviewsPage()
	{
		this.InitializeComponent();

		Opening += ApplyRightDrawerFlyoutPresenterStyle;
	}
}
