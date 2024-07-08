---
uid: Uno.Recipes.BasicLightweightStyling
---

# How to Apply Basic Lightweight Styling

## Problem

Applying consistent and efficient styles across an application can be challenging, especially when dealing with complex UI elements. Lightweight styling allows for a more modular and efficient approach to applying styles.

## Solution

**Uno.Toolkit** provides lightweight styling capabilities, which allow you to create reusable styles that can be easily applied across your application. This approach helps in creating a consistent look and feel by allowing you to override resources at different levels, such as app-level, page-level, or control-level.

1. App-Level Resource Overrides:

    At the app level, you can override certain resources using basic lightweight styling. These overrides will apply to the entire application, here is an example from the `Chefs` app:

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

## Source Code

Chefs app

- [App.xaml](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/App.xaml)

- [Login.xaml](https://github.com/unoplatform/uno.chefs/blob/6edfea34e5adc1245f0d0ae1c71c1b0193d15b06/src/Chefs/Views/LoginPage.xaml#L17-L30)

## Documentation

- [Uno Toolkit Resource Extensions](xref:Uno.Toolkit.Helpers.ResourceExtensions)
- [Uno Toolkit Lightweight Styling](xref:Uno.Toolkit.LightweightStyling)
