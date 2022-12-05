using Uno.Toolkit.UI;

namespace Chefs.Views;

public sealed partial class FilterPage : Flyout
{
    public FilterPage()
    {
        this.InitializeComponent();

        Opening += FilterPage_Opening;
    }

    private void FilterPage_Opening(object? sender, object e)
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
