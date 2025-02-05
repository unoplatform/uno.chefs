---
uid: Uno.Recipes.LightweightStyling
---

# How to Apply Lightweight Styling

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

Chefs app

- [App.xaml](https://github.com/unoplatform/uno.chefs/blob/023f952fa87a47ee9edb85eeecf351a17a002477/src/Chefs/App.xaml)

- [Login.xaml](https://github.com/unoplatform/uno.chefs/blob/6edfea34e5adc1245f0d0ae1c71c1b0193d15b06/src/Chefs/Views/LoginPage.xaml#L17-L30)

## Documentation

- [Uno Toolkit Lightweight Styling](xref:Uno.Toolkit.LightweightStyling)
- [Uno Themes Lightweight Styling](xref:Uno.Themes.LightweightStyling)
