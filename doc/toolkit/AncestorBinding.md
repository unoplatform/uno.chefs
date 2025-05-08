---
uid: Uno.Recipes.AncestorBinding
---

# Binding to Elements Outside of a DataTemplate in XAML

## Problem

It is not always possible to access elements outside of a `DataTemplate` using the `ElementName` binding. This is a common scenario when you want to bind to a property of the parent `DataContext` from within the `ItemTemplate` of something like an `ItemsControl`.

## Solution

The `AncestorBinding` markup extension in the Uno Toolkit provides a way to find the nearest ancestor of a specific type and bind to its properties.

### Example Code

```xml
<uer:FeedView Source="{Binding TrendingNow}">
    <muxc:ItemsRepeater ItemsSource="{Binding Data}">
        <muxc:ItemsRepeater.ItemTemplate>
            <DataTemplate>
                <ToggleButton Style="{StaticResource IconToggleButtonStyle}"
                              IsChecked="{Binding IsFavorite}"
                              Command="{utu:AncestorBinding AncestorType=uer:FeedView, Path=DataContext.FavoriteRecipe}"
                              CommandParameter="{Binding}">
                </ToggleButton>
            </DataTemplate>
        </muxc:ItemsRepeater.ItemTemplate>
    </muxc:ItemsRepeater>
</uer:FeedView>
```

## Source Code

Chefs app

- [Home Page](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/HomePage.xaml#L46-L50)
- [Create/Update Cookbook Page](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/CreateUpdateCookbookPage.xaml#L71-L80)
- [Recipe Details Page (1)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RecipeDetailsPage.xaml#L347-L348)
- [Recipe Details Page (2)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RecipeDetailsPage.xaml#L378-L379)

## Documentation

- [AncestorBinding documentation](xref:Toolkit.Helpers.Bindings)
