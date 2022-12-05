namespace Chefs.Views;

public sealed partial class ReviewsPage : Flyout
{
    public ReviewsPage()
    {
        this.InitializeComponent();
        Opening += ReviewsPage_Opening;
    }

    private async void ReviewsPage_Opening(object? sender, object e)
    {
        if (Window.Current.Bounds.Width > 700)
        {
            if (App.Current.Resources.TryGetValue("CustomRightDrawerFlyoutPresenterStyle", out var style) && style is Style)
            {
                FlyoutPresenterStyle = style as Style;
            }
        }
        else
        {
            SetValue(FlyoutPresenterStyleProperty, DependencyProperty.UnsetValue);
        }
    }
}
