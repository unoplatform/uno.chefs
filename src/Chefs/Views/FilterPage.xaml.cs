namespace Chefs.Views;

public sealed partial class FilterPage : ResponsiveDrawerFlyout
{
	public FilterPage()
	{
		this.InitializeComponent();

		Opening += ApplyRightDrawerFlyoutPresenterStyle;
	}
}