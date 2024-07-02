---
uid: Uno.Recipes.NavigationShell
---

# How to Create a Navigation Shell 

## Problem

Creating a navigation shell that hosts multiple pages and maintains a consistent navigation experience can be challenging. Ensuring that the main navigation structure is always accessible while allowing dynamic content to be displayed requires a robust setup.

## Creating a Navigation Shell

**Uno.Toolkit** library provides the tools needed to create a navigation shell with a main page that hosts other pages in a region-attached Grid and a TabBar that remains visible at the bottom. This setup provides a consistent and intuitive navigation experience.

### Define the MainPage with a Grid to host the navigation content and a TabBar for navigation.

Here is the `MainPage.xaml` from the Chefs app.

``` xml
<Page x:Class="Chefs.Views.MainPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      xmlns:uen="using:Uno.Extensions.Navigation.UI"
      xmlns:utu="using:Uno.Toolkit.UI"
      Background="{ThemeResource BackgroundBrush}">

    <Grid>
        <Grid x:Name="MainGrid"
              RowSpacing="0"
              Background="{ThemeResource SurfaceBrush}"
              uen:Region.Attached="True"
              utu:AutoLayout.CounterAlignment="Start"
              utu:AutoLayout.PrimaryAlignment="Stretch">
            
            <Grid.RowDefinitions>
                <RowDefinition />
                <RowDefinition Height="Auto" />
            </Grid.RowDefinitions>

            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>

            <Grid x:Name="NavigationGrid"
                  Grid.Row="0"
                  Grid.Column="1"
                  uen:Region.Attached="True"
                  uen:Region.Navigator="Visibility" />
            
            <utu:TabBar x:Name="Tabs"
                        Grid.Row="1"
                        Grid.Column="1"
                        Visibility="{utu:Responsive Narrow=Visible, Wide=Collapsed}"
                        uen:Region.Attached="True"
                        Style="{StaticResource BottomTabBarStyle}">
                <utu:TabBarItem uen:Region.Name="Home"
                                IsSelectable="True"
                                Content="Home"
                                Foreground="{ThemeResource OnSurfaceVariantBrush}">
                    <utu:TabBarItem.Icon>
                        <FontIcon Glyph="{StaticResource Icon_Home}" />
                    </utu:TabBarItem.Icon>
                </utu:TabBarItem>

                <utu:TabBarItem uen:Region.Name="Search"
                                IsSelectable="True"
                                Content="Search"
                                Foreground="{ThemeResource OnSurfaceVariantBrush}">
                    <utu:TabBarItem.Icon>
                        <FontIcon Glyph="{StaticResource Icon_Search}" />
                    </utu:TabBarItem.Icon>
                </utu:TabBarItem>

                <utu:TabBarItem uen:Region.Name="FavoriteRecipes"
                                IsSelectable="True"
                                Content="Favorites"
                                Foreground="{ThemeResource OnSurfaceVariantBrush}">
                    <utu:TabBarItem.Icon>
                        <FontIcon Glyph="{StaticResource Icon_Heart}" />
                    </utu:TabBarItem.Icon>
                </utu:TabBarItem>
            </utu:TabBar>
        </Grid>
    </Grid>
</Page>
```

**Main Grid**: The MainGrid hosts the navigation content and the TabBar.

- `uen:Region.Attached="True"`: Attaches the region to enable navigation.
- `utu:AutoLayout.CounterAlignment="Start"` and `utu:AutoLayout.PrimaryAlignment="Stretch"`: Ensures the layout is responsive and aligned correctly.

**NavigationGrid**: Hosts the pages within the main content area.

- `uen:Region.Attached="True"`: Attaches the region to enable navigation within this grid.
- `uen:Region.Navigator="Visibility"`: Ensures the visibility of the navigator.

**TabBar**: Provides navigation tabs at the bottom.

- `uen:Region.Name`: Specifies the region name for navigation.
- `utu:TabBarItem`: Defines individual tab items with icons and labels.

Navigation Shell Structure:

- `MainPage`: Acts as the shell hosting the TabBar and the NavigationGrid.
- `TabBar`: Always visible at the bottom, providing consistent navigation.
- `NavigationGrid`: Dynamically displays the content of the selected page.

For more detailed information and advanced use cases of navigation with TabBar, refer to the [Advanced TabBar Navigation documentation](xref:Uno.Extensions.Navigation.Advanced.TabBar).

### Example Usage in Workshop

An example usage of the ThemeService can be found in the [Simple Calc workshop](https://platform.uno/docs/articles/external/workshops/simple-calc/modules/MVVM-XAML/05-Finish%20the%20App/README.html#adding-the-themeservice).


## Source Code

Chefs app

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/App.cs#L43)

- [SettingsModel](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/SettingsModel.cs#L27-L28)
