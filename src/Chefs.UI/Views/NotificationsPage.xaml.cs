using Windows.UI.Core;

namespace Chefs.Views;

public sealed partial class NotificationsPage : Flyout
{
    public NotificationsPage()
    {
        this.InitializeComponent();

        Opening += NotificationsPage_Opening;
    }

    private async void NotificationsPage_Opening(object? sender, object e)
    {
        if (Window.Current.Bounds.Width > 700)
        {
            if (App.Current.Resources.TryGetValue("CustomLeftDrawerFlyoutPresenterStyle", out var style) && style is Style)
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
