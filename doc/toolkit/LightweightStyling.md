---
uid: Uno.Recipes.LightweightStyling
---

# How to Utilize Lightweight Styling

## Problem

Applying consistent and efficient styles across an application can be challenging, especially when dealing with complex UI elements. Lightweight styling allows for a more modular and efficient approach to applying styles.

## Solution

**Uno.Toolkit** provides a lightweight styling mechanism using the Resources attached property. This allows for a clean and maintainable way to manage styles across your application.



### Applying Lightweight Styling

In the `Chefs` app, we can utilize lightweight styling to ensure a consistent look and feel across different controls. This is particularly useful for styling complex controls.

1. Define Styles in App.xaml:

    In the `App.xaml` file, we define the necessary resources and styles using the `ResourceDictionary`.

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
                <Style TargetType="utu:CardContentControl"  x:Key="FilledCardContentControlStyle">
                    <Setter Property="Background" Value="{ThemeResource SurfaceBrush}" />
                     <Setter Property="CornerRadius" Value="4" />
                </Style>
            </ResourceDictionary>
        </Application.Resources>
    </Application>
    ```

2. Apply Lightweight Styles in a Page:

    On the `HomePage`, use the defined styles for controls like `CardContentControl` and `ToggleButton`.

    ```xml
    <Page x:Class="Chefs.Views.HomePage"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:utu="using:Uno.Toolkit.UI"
        Background="{ThemeResource BackgroundBrush}">
        
        <Page.Resources>
            <DataTemplate x:Key="HomeLargeItemTemplate">
                <utu:CardContentControl Style="{StaticResource FilledCardContentControlStyle}">
                    <utu:AutoLayout Background="{ThemeResource SurfaceBrush}"
                                    CornerRadius="4"
                                    PrimaryAxisAlignment="Center"
                                    HorizontalAlignment="Stretch">
                        <utu:AutoLayout CornerRadius="4">
                            <Border Height="144">
                                <Image HorizontalAlignment="Center"
                                    VerticalAlignment="Center"
                                    Source="{Binding ImageUrl}"
                                    Stretch="UniformToFill" />
                            </Border>
                            <utu:AutoLayout Spacing="16"
                                            Padding="16"
                                            Justify="SpaceBetween"
                                            Orientation="Horizontal">
                                <utu:AutoLayout Spacing="4">
                                    <TextBlock TextWrapping="Wrap"
                                            Text="{Binding Name}"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource TitleSmall}" />
                                    <TextBlock TextWrapping="Wrap"
                                            Text="{Binding TimeCal}"
                                            Foreground="{ThemeResource OnSurfaceMediumBrush}"
                                            Style="{StaticResource CaptionMedium}" />
                                </utu:AutoLayout>
                                <ToggleButton Style="{StaticResource IconToggleButtonStyle}"
                                            IsChecked="{Binding IsFavorite}"
                                            Command="{utu:AncestorBinding AncestorType=uer:FeedView,
                                                                            Path=DataContext.FavoriteRecipe}"
                                            CommandParameter="{Binding}">
                                    <ToggleButton.Content>
                                        <FontIcon Style="{StaticResource FontAwesomeRegularFontIconStyle}"
                                                Glyph="{StaticResource Icon_Favorite}"
                                                Foreground="{ThemeResource OnSurfaceBrush}" />
                                    </ToggleButton.Content>
                                    <ut:ControlExtensions.AlternateContent>
                                        <FontIcon Style="{StaticResource FontAwesomeSolidFontIconStyle}"
                                                Glyph="{StaticResource Icon_Favorite}"
                                                Foreground="{ThemeResource PrimaryBrush}" />
                                    </ut:ControlExtensions.AlternateContent>
                                </ToggleButton>
                            </utu:AutoLayout>
                        </utu:AutoLayout>
                    </utu:AutoLayout>
                </utu:CardContentControl>
            </DataTemplate>
        </Page.Resources>

        <!-- Page content -->
    </Page>
    ```

3. Utilizing `Resource` attached property for Styling:

    Define styles directly within a control using the Resources attached property.

    ```xml
    <utu:CardContentControl>
        <utu:CardContentControl.Resources>
            <Style TargetType="TextBlock">
                <Setter Property="Foreground" Value="{ThemeResource OnSurfaceBrush}" />
                <Setter Property="FontSize" Value="16" />
            </Style>
        </utu:CardContentControl.Resources>
        <!-- Content here will inherit the styles defined in the Resources property -->
    </utu:CardContentControl>
    ```

## Source Code

Chefs app

- [App.xaml](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/App.xaml)

- [Home Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/HomePage.xaml)

## Documentation

- [Uno Toolkit Resource Extensions](xref:Uno.Toolkit.Helpers.ResourceExtensions)
- [Uno Toolkit Lightweight Styling](xref:Uno.Toolkit.LightweightStyling)
