---
uid: Uno.Recipes.DisplayDrawerFlyout
---
# How to Display a Drawer Flyout for Screen Space Management

## Problem

In applications, especially on mobile, managing screen space efficiently is crucial. A common UI pattern is a drawer flyout, which can house navigation links or other content without permanently taking up screen real estate.

## Solution

The `DrawerFlyout` from the **Uno.Toolkit** provides a versatile solution for implementing adaptive drawer flyouts.

- Basic Drawer Flyout Example

    To start with a basic drawer flyout, you can use one of the FlyoutPresenterStyles provided by the Uno Toolkit. Here’s how you can create a simple drawer flyout:

    ```xml
    <Button Content="Open Flyout">
        <Button.Flyout>
            <Flyout Placement="Left" FlyoutPresenterStyle="{StaticResource LeftDrawerFlyoutPresenterStyle}">
            <TextBlock Text="This is a basic flyout with Uno Toolkit Style!" Margin="12" />
            </Flyout>
        </Button.Flyout>
    </Button>
    ```

    This example uses `LeftDrawerFlyoutPresenterStyle`, one of the pre-built styles from Uno Toolkit, which configures the Flyout to open from the left with a drawer-like experience.

### Customizing the Drawer Flyout

- Creating the ResponsiveDrawerFlyout:

    ```xml
    <Flyout x:Class="Chefs.Views.Flyouts.ResponsiveDrawerFlyout"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:toolkit="using:Uno.UI.Toolkit"
            Placement="Full"
            FlyoutPresenterStyle="{StaticResource BottomDrawerFlyoutPresenterStyle}">
        <Grid>
            <!-- Content here -->
        </Grid>
    </Flyout>
    ```

- The drawer can be adjusted on the fly to provide an optimal user experience:

    ```csharp
    public ResponsiveDrawerFlyout()
    {
        InitializeComponent();
        Opening += OnOpening;
    }

    private void OnOpening(object? sender, object e)
    {
        if (_presenter is { } presenter)
        {
            var width = XamlRoot?.Size.Width ?? 0;
            if (width >= WideBreakpoint)
            {
                DrawerFlyoutPresenter.SetDrawerLength(presenter, new GridLength(0.33, GridUnitType.Star));
                DrawerFlyoutPresenter.SetOpenDirection(presenter, DrawerOpenDirection.Left);
                DrawerFlyoutPresenter.SetIsGestureEnabled(presenter, false);
            }
            else
            {
                DrawerFlyoutPresenter.SetDrawerLength(presenter, new GridLength(1, GridUnitType.Star));
                DrawerFlyoutPresenter.SetIsGestureEnabled(presenter, false);
            }
        }
    }
    ```

- Injecting Custom Flyout for Navigation:

    To ensure the `ResponsiveDrawerFlyout` is used consistently across the application, it is injected into the navigation service configuration in `App.xaml.host.cs`:

    ```csharp
    private void ConfigureNavServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddTransient<Flyout, ResponsiveDrawerFlyout>();
    }
    ```

    Here's an example of invoking a modal navigation with the "!" qualifier from the Chefs app:

    ```csharp
    // Navigation method example
    public static Task NavigateToNotifications(this INavigator navigator, object sender)
    {
        return navigator.NavigateRouteAsync(sender, "Notifications", qualifier: Qualifiers.Dialog);
    }

    // Qualifiers definition
    public static class Qualifiers
    {
        public const string Dialog = "!"; 
    }
    ```

## Example Usage in Chefs

The Chefs app uses a **ResponsiveDrawerFlyout** to provide a flexible navigation drawer that adapts to the application's state and the device's orientation. Refer to the **Source Code** section for the implementation details.

## Source Code

- [Responsive Drawer Flyout XAML](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/Flyouts/ResponsiveDrawerFlyout.xaml)
- [ResponsiveDrawerFlyout Code-Behind](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/Flyouts/ResponsiveDrawerFlyout.xaml.cs)

## Documentation

- [Drawer Flyout Presenter](xref:Toolkit.Controls.DrawerFlyoutPresenter)
