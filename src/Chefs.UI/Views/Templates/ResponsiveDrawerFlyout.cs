namespace Chefs.Views;

public abstract class ResponsiveDrawerFlyout : Flyout
{
    protected virtual string WideScreenLeftPresenterStyle => "CustomLeftDrawerFlyoutPresenterStyle";
    protected virtual string WideScreenRightPresenterStyle => "CustomRightDrawerFlyoutPresenterStyle";

    public async void ApplyLeftDrawerFlyoutPresenterStyle(object? sender, object e)
    {
        if (Window.Current.Bounds.Width > 700)
        {
            if (App.Current.Resources.TryGetValue(WideScreenLeftPresenterStyle, out var result) && result is Style style)
            {
                FlyoutPresenterStyle = style;
            }
        }
        else
        {
            ClearValue(FlyoutPresenterStyleProperty);
        }
    }

    public async void ApplyRightDrawerFlyoutPresenterStyle(object? sender, object e)
    {
        if (Window.Current.Bounds.Width > 700)
        {
            if (App.Current.Resources.TryGetValue(WideScreenRightPresenterStyle, out var result) && result is Style style)
            {
                FlyoutPresenterStyle = style;
            }
        }
        else
        {
            ClearValue(FlyoutPresenterStyleProperty);
        }
    }
}