---
uid: Uno.Recipes.ResourceExtensions
---

# How to utilize the Resource Extensions

## Problem

Applying consistent and efficient styles across an application can be challenging, especially when dealing with complex UI elements. Lightweight styling allows for a more modular and efficient approach to applying styles.

## Solution

**Uno.Toolkit** provides a lightweight styling mechanism using the Resources Extensions. This extension facilitates assigning a specific ResourceDictionary directly to a control's style. It simplifies [lightweight styling](xref:Uno.Toolkit.LightweightStyling ) by eliminating the necessity to declare each resource on the page explicitly, enabling the easy creation of diverse visual elements with shared styles but varied attributes.


### Applying Resource Extensions

In the `Chefs` app, we can utilize the `ResourceExtensions.Resources` attached property to ensure a consistent look and feel across different controls.

1. App-Level Resource Overrides:

    At the app level, you can override certain resources using basic lightweight styling. These overrides will apply to the entire application..

    ```xml
    <Application x:Class="Chefs.App"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:utu="using:Uno.Toolkit.UI">
        <Application.Resources>
            <ResourceDictionary>
                <ResourceDictionary.MergedDictionaries>
                    <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
                    <MaterialToolkitTheme xmlns="using:Uno.Toolkit.UI.Material"
                                    ColorOverrideSource="ms-appx:///Styles/ColorPaletteOverride.xaml"
                                    FontOverrideSource="ms-appx:///Styles/MaterialFontsOverride.xaml" />
                    <!-- Additional resource dictionaries -->
                </ResourceDictionary.MergedDictionaries>
                <ResourceDictionary.ThemeDictionaries>
                    <ResourceDictionary x:Key="Light">
                        <StaticResource x:Key="FabBackground" ResourceKey="PrimaryBrush" />
                        <StaticResource x:Key="FabBackgroundPressed" ResourceKey="PrimaryBrush" />
                        <StaticResource x:Key="FabBackgroundPointerOver" ResourceKey="PrimaryBrush" />
                    </ResourceDictionary>
                    <ResourceDictionary x:Key="Dark">
                        <StaticResource x:Key="FabBackground" ResourceKey="PrimaryBrush" />
                        <StaticResource x:Key="FabBackgroundPressed" ResourceKey="PrimaryBrush" />
                        <StaticResource x:Key="FabBackgroundPointerOver" ResourceKey="PrimaryBrush" />
                    </ResourceDictionary>
                </ResourceDictionary.ThemeDictionaries>
            </ResourceDictionary>
        </Application.Resources>
    </Application>
    ```

2. Page-Level Resource Overrides:

    At the page level, you can override resources, which will apply to all controls within that page, here is an example from the `LoginPage` in Chefs:

    ```xml
    <Page.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key="Light">
                    <StaticResource x:Key="OutlinedPasswordBoxPlaceholderForeground" ResourceKey="OnSurfaceMediumBrush" />
                </ResourceDictionary>
                <ResourceDictionary x:Key="Default">
                    <StaticResource x:Key="OutlinedPasswordBoxPlaceholderForeground" ResourceKey="OnSurfaceMediumBrush" />
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </Page.Resources>    
    ```

3. Using the `ResourceExtensions.Resources` property to define a custom style:

    The `Fab.xaml` file from Chefs demonstrates how to define a custom style with resource overrides packaged as part of the style using the `ResourceExtensions.Resources` attached property:

    ```xml
    <Setter Property="utu:ResourceExtensions.Resources">
                <Setter.Value>
                    <ResourceDictionary>
                        <ResourceDictionary.ThemeDictionaries>
                            <ResourceDictionary x:Key="Default">
                                <StaticResource x:Key="FabForeground" ResourceKey="OnPrimaryBrush" />
                                <StaticResource x:Key="FabForegroundPressed" ResourceKey="OnPrimaryBrush" />
                                <StaticResource x:Key="FabForegroundPointerOver" ResourceKey="OnPrimaryBrush" />
                                <StaticResource x:Key="FabForegroundDisabled" ResourceKey="OnSurfaceDisabledBrush" />
                                <StaticResource x:Key="FabBackground" ResourceKey="PrimaryBrush" />
                                <StaticResource x:Key="FabBackgroundPressed" ResourceKey="PrimaryBrush" />
                                <StaticResource x:Key="FabBackgroundPointerOver" ResourceKey="PrimaryBrush" />
                                <StaticResource x:Key="FabBackgroundDisabled" ResourceKey="SystemControlTransparentBrush" />
                                <StaticResource x:Key="FabStateOverlayBackground" ResourceKey="SystemControlTransparentBrush" />
                                <StaticResource x:Key="FabStateOverlayBackgroundPointerOver" ResourceKey="OnPrimaryHoverBrush" />
                                <StaticResource x:Key="FabStateOverlayBackgroundFocused" ResourceKey="OnPrimaryFocusedBrush" />
                                <StaticResource x:Key="FabStateOverlayBackgroundPressed" ResourceKey="OnPrimaryPressedBrush" />
                            </ResourceDictionary>
                            <ResourceDictionary x:Key="Light">
                                <StaticResource x:Key="FabForeground" ResourceKey="OnPrimaryBrush" />
                                <StaticResource x:Key="FabForegroundPressed" ResourceKey="OnPrimaryBrush" />
                                <StaticResource x:Key="FabForegroundPointerOver" ResourceKey="OnPrimaryBrush" />
                                <StaticResource x:Key="FabForegroundDisabled" ResourceKey="OnSurfaceDisabledBrush" />
                                <StaticResource x:Key="FabBackground" ResourceKey="PrimaryBrush" />
                                <StaticResource x:Key="FabBackgroundPressed" ResourceKey="PrimaryBrush" />
                                <StaticResource x:Key="FabBackgroundPointerOver" ResourceKey="PrimaryBrush" />
                                <StaticResource x:Key="FabBackgroundDisabled" ResourceKey="SystemControlTransparentBrush" />
                                <StaticResource x:Key="FabStateOverlayBackground" ResourceKey="SystemControlTransparentBrush" />
                                <StaticResource x:Key="FabStateOverlayBackgroundPointerOver" ResourceKey="OnPrimaryHoverBrush" />
                                <StaticResource x:Key="FabStateOverlayBackgroundFocused" ResourceKey="OnPrimaryFocusedBrush" />
                                <StaticResource x:Key="FabStateOverlayBackgroundPressed" ResourceKey="OnPrimaryPressedBrush" />
                            </ResourceDictionary>
                        </ResourceDictionary.ThemeDictionaries>
                    </ResourceDictionary>
                </Setter.Value>
            </Setter>
    ```

## Source Code

Chefs app

- [App.xaml](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/App.xaml)

- [Fab.xaml](https://github.com/unoplatform/uno.chefs/blob/6edfea34e5adc1245f0d0ae1c71c1b0193d15b06/src/Chefs/Styles/Fab.xaml#L11-L46)

- [Login.xaml](https://github.com/unoplatform/uno.chefs/blob/6edfea34e5adc1245f0d0ae1c71c1b0193d15b06/src/Chefs/Views/LoginPage.xaml#L17-L30)

## Documentation

- [Uno Toolkit Resource Extensions](xref:Uno.Toolkit.Helpers.ResourceExtensions)
- [Uno Toolkit Lightweight Styling](xref:Uno.Toolkit.LightweightStyling)
