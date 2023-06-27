namespace Chefs.Views;

public sealed partial class FilterPage : ResponsiveDrawerFlyout
{
	public FilterPage()
	{
		this.InitializeComponent();

		Opening += ApplyRightDrawerFlyoutPresenterStyle;
	}

    private void MainLayout_Loaded(object sender, RoutedEventArgs e)
    {
        MainLayout.MinWidth = GetMinWidth();
    }

    public double GetMinWidth()
    {
#if __IOS__
        return UIScreen.MainScreen.Bounds.Width;
#else
        return 0;
#endif
    }
}