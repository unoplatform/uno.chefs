---
uid: Uno.Recipes.DisplayDrawerFlyout
---
# How to Display a Drawer Flyout

## Problem
In applications, especially on mobile, managing screen space efficiently is crucial. A common UI pattern is a drawer flyout, which can house navigation links or other content without permanently taking up screen real estate.

## Solution
The `DrawerFlyout` from the **Uno.Toolkit** provides a versatile solution for implementing adaptive drawer flyouts.

* Creating the ResponsiveDrawerFlyout: 

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

* The drawer can be adjusted on the fly to provide an optimal user experience:

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

* Injecting Custom Flyout for Navigation:

    To ensure the `ResponsiveDrawerFlyout` is used consistently across the application, it is injected into the navigation service configuration in `App.xaml.cs.` This setup uses the **"!"** qualifier to specify when the custom flyout should be employed:

    ```csharp
    private void ConfigureNavServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddTransient<Flyout, ResponsiveDrawerFlyout>();
    }
    ```

## Example Usage in Chefs
The Chefs app implements a [ResponsiveDrawerFlyout]() to provide a flexible navigation drawer that adjusts based on the application state and device orientation.

## Source Code
- [Responsive Drawer Flyout XAML](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Views/Flyouts/ResponsiveDrawerFlyout.xaml)
- [ResponsiveDrawerFlyout Code-Behind](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Views/Flyouts/ResponsiveDrawerFlyout.xaml.cs)

## Documentation
- [Drawer Flyout Presenter](xref:Toolkit.Controls.DrawerFlyoutPresenter)