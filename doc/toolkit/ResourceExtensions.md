---
uid: Uno.Recipes.ResourceExtensions
---

# How to utilize the Resource Extensions

## Problem

Applying consistent and efficient styles across an application can be challenging, especially when dealing with complex UI elements. Lightweight styling allows for a more modular and efficient approach to applying styles.

## Solution

**Uno.Toolkit** provides a lightweight styling mechanism using the Resources Extensions. This extension facilitates assigning a specific ResourceDictionary directly to a control's style. It simplifies [lightweight styling](xref:Toolkit.LightweightStyling) by eliminating the necessity to declare each resource on the page explicitly, enabling the easy creation of diverse visual elements with shared styles but varied attributes.


### Applying Resource Extensions

In the `Chefs` app, we can utilize the `ResourceExtensions.Resources` attached property to ensure a consistent look and feel across different controls.

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

For more detailed examples of basic lightweight styling, please refer to the [Basic Lightweight Styling recipe book](xref:Uno.Recipes.LightweightStyling).

## Source Code

Chefs app

- [Fab Style](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Styles/Button.xaml#L45-L80)

## Documentation

- [Uno Toolkit Resource Extensions](xref:Toolkit.Helpers.ResourceExtensions)
- [Uno Toolkit Lightweight Styling](xref:Toolkit.LightweightStyling)