---
uid: Uno.Recipes.LightweightStyling
---

# Lightweight Styling for UI Elements

## Problem

Applying consistent and efficient styles across an application can be challenging, especially when dealing with complex UI elements. Lightweight styling allows for a more modular and efficient approach to applying styles. [Lightweight Styling.](learn.microsoft.com/en-us/windows/apps/develop/platform/xaml/xaml-styles#lightweight-styling)

## Solution

Lightweight styling allows you to create reusable styles that can be applied across your application. This is a general capability of XAML styles, where you can define ThemeResources to achieve this. Both **Uno.Toolkit** and **Uno.Themes** provide styles that work seamlessly with lightweight styling, making it easier to maintain consistency across your app.

1. App-Level Resource Overrides:

    At the app level, you can override certain resources using lightweight styling. These overrides will apply to the entire application, here is an example from the `Chefs` app:

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

                    <ResourceDictionary Source="ms-appx:///Converters/Converters.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/FeedView.xaml" />
                    <ResourceDictionary Source="ms-appx:///Views/Templates/ItemTemplates.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/Strings.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/CustomFonts.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/ChartBrushes.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/Images.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/Button.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/TextBox.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/NavigationBar.xaml" />
                    <ResourceDictionary Source="ms-appx:///Styles/Page.xaml" />
                    <ResourceDictionary>
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
                </ResourceDictionary.MergedDictionaries>
            </ResourceDictionary>
        </Application.Resources>
    </Application>
    ```

    To see all the available resources for styling FloatingActionButton (FAB), refer to the Uno Themes [FloatingActionButton]( xref:Uno.Themes.Styles.FloatingActionButton).

2. Control-Level Resource Overrides:

   You can also apply lightweight styling directly on specific controls for more granular control over their appearance. For example, in the `LoginPage` in the Chefs app, the `OutlinedPasswordBoxPlaceholderForeground` resource is overridden directly on the `PasswordBox` control:

    ```xml
    <PasswordBox.Resources>
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
    </PasswordBox.Resources>
    ```

## Source Code

- [App.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml)
- [Login.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/LoginPage.xaml#L46-L59)

## Documentation

- [Uno Toolkit Lightweight Styling](xref:Toolkit.LightweightStyling)
- [Uno Themes Lightweight Styling](xref:Uno.Themes.LightweightStyling)
