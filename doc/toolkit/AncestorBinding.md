---
uid: Uno.Recipes.AncestorBinding
---

# How to bind to elements outside of a DataTemplate in XAML

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

- [Home Page](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/HomePage.xaml#L46-L50)
- [Create/Update Cookbook Page](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/CreateUpdateCookbookPage.xaml#L71-L80)
- [Recipe Details Page (1)](https://github.com/unoplatform/uno.chefs/blob/e02a4dce407e13b933d2e8e6c764d237ebc11d33/src/Chefs/Views/RecipeDetailsPage.xaml#L336)
- [Recipe Details Page (2)](https://github.com/unoplatform/uno.chefs/blob/e02a4dce407e13b933d2e8e6c764d237ebc11d33/src/Chefs/Views/RecipeDetailsPage.xaml#L379)
- [Recipe Details Page (3)](https://github.com/unoplatform/uno.chefs/blob/e02a4dce407e13b933d2e8e6c764d237ebc11d33/src/Chefs/Views/RecipeDetailsPage.xaml#L719)
- [Recipe Details Page (4)](https://github.com/unoplatform/uno.chefs/blob/e02a4dce407e13b933d2e8e6c764d237ebc11d33/src/Chefs/Views/RecipeDetailsPage.xaml#L758)
- [Live Cooking Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9https://github.com/unoplatform/uno.chefs/blob/e02a4dce407e13b933d2e8e6c764d237ebc11d33/src/Chefs/Views/LiveCookingPage.xaml#L251)

## Documentation

- [AncestorBinding documentation](xref:Toolkit.Helpers.Bindings)
