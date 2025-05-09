---
uid: Uno.Recipes.NavigationShell
---

# Creating a Responsive Navigation Shell

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
                        Visibility="{utu:Responsive Normal=Visible, Wide=Collapsed}"
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

### Making the Navigation Shell Responsive

In `MainPage.xaml`, we define two TabBar elements with different layouts:

- Horizontal TabBar: Visible when the window is Normal.

- Vertical TabBar: Visible when the window is wide.

``` xml
<!-- Horizontal TabBar for smaller screens -->
<utu:TabBar Grid.Row="1"
            Grid.Column="1"
            Visibility="{utu:Responsive Normal=Visible, Wide=Collapsed}"
            uen:Region.Attached="True"
            Style="{StaticResource BottomTabBarStyle}">
</utu:TabBar>

<!-- Vertical TabBar for wide screens -->
<utu:AutoLayout Grid.RowSpan="2"
                Background="{ThemeResource SurfaceBrush}"
                Visibility="{utu:Responsive Normal=Collapsed, Wide=Visible}"
                Width="120">
<utu:TabBar uen:Region.Attached="True"
            Style="{StaticResource VerticalTabBarStyle}"
            utu:AutoLayout.PrimaryAlignment="Stretch">
</utu:TabBar>
</utu:AutoLayout>
```

The [ResponsiveExtension](xref:Uno.Recipes.ResponsiveExtension) from `Uno.Toolkit` dynamically adjusts the Visibility of each TabBar based on the window size.

For more detailed information and advanced use cases of navigation with TabBar, refer to the [Advanced TabBar Navigation documentation](xref:Uno.Extensions.Navigation.Advanced.TabBar).

## Source Code

- [MainPage.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/MainPage.xaml)
