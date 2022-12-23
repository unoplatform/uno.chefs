namespace Chefs.Views;

public sealed partial class ProfilePage : Flyout
{
    public ProfilePage()
    {
        this.InitializeComponent();

        Opening += ProfilePage_Opening;
    }

    private void ProfilePage_Opening(object? sender, object e)
    {
        if (Window.Current.Bounds.Width > 700)
        {
            if (App.Current.Resources.TryGetValue("RightDrawerFlyoutPresenterStyle", out var style) && style is Style)
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
