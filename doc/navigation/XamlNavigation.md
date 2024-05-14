---
uid: Uno.Recipes.XamlNavigation
---

# How-To: Navigate in Xaml

## Problem

Navigation implementations vary between platforms, and it can be time-consuming to create and manage all the different navigation methods with each their own destination and data.

## Solution

Uno Navigation simplifies development by abstracting away platform-specific navigation implementations and promotes code reusability, which allows you to focus on building features rather than managing navigation intricacies.

### Chefs Example

We'll want to take a deeper look into the `Navigation.Request` and `Navigation.Data` attached properties. On the Chefs home page there are some `ItemRepeater` controls that each display a list of filtered clickable recipes. When a user clicks on a recipe, they are brought to that specific recipe's details page. Let's look at how the `Request` and `Data` properties are used:

### 1. Navigation.Request

First, let's concentrate on the `Navigation.Request` attached property.

```xml
<muxc:ItemsRepeater ItemsSource="{Binding Data}"
					uen:Navigation.Request="RecipeDetails"
					uen:Navigation.Data="{Binding Data}"
					ItemTemplate="{StaticResource HomeLargeItemTemplate}">
```

The string value specified is the route to be navigated to, here _RecipeDetails_. When the user clicks on an item in the `ItemRepeater`, the app will find the corresponding page for this route and redirect the user accordingly. Different events are triggered for different types of controls, you can find more information [here](xref:Uno.Extensions.Navigation.HowToNavigateInXAML#1-navigationrequest).

> [!TIP]
> As Navigation.Request attached property exists in the `Uno.Extensions.Navigation.UI` namespace you will need to import this namespace on the `Page` element with

```csharp
<Page x:Class="Chefs.Views.HomePage"
	  ...
	  xmlns:uen="using:Uno.Extensions.Navigation.UI">
```

A conventional route that is defined for navigating back is "-". Here's an example from the _ReviewsPage_:

```xml
<Button uen:Navigation.Request="-"
		utu:AutoLayout.CounterAlignment="End"
	    Style="{StaticResource IconButtonStyle}">
```

### 2. Navigation.Data

Navigating with data is made easy with the `Navigation.Data` attached property. We can define the data to be attached to the navigation request, which can be accessed by the route's view model using constructor injection. Let's see how the _RecipeDetails_ page receives the correct data when a user clicks on a specific recipe.

First of all, we're going to need to define the type of data that will be attached to the navigation request. Here, it's a recipe record:

```csharp
public partial record Recipe : IChefEntity
{
    public string? ImageUrl { get; init; }
	public string? Name { get; init; }
	public int Serves { get; init; }
    ...
}
```

When we're registering the routes in the _App.cs_ file, we should add a `DataMap` parameter to the `ViewMap` for _RecipeDetailsPage_. This allows the Recipe to be injected into the _RecipeDetailsModel_ during navigation.

```csharp
private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
{
    views.Register(
        /* other ViewMaps */,
        new ViewMap<RecipeDetailsPage, RecipeDetailsModel>(Data: new DataMap<Recipe>())
    );

    ...
}
```

We also have to update the _RecipeDetailsModel_ to accept a Recipe as its first constructor parameter. This is the parameter that will hold the _Data_ from the binding.

```csharp
public partial class RecipeDetailsModel
{
	...

	public RecipeDetailsModel(Recipe recipe, ...)
	{
		...

		Recipe = recipe;
	}

	public Recipe Recipe { get; }

    ...
}
```

If we look back at the navigation request, we can see that the `ItemsSource` is bound to the Data of the current context, which is from the `FeedView` control's `Source` _TrendingNow_ (of type `IListFeed<Recipe>`). Therefore, we will get a list of recipes, and clicking on a specific recipe will bring the user to the _RecipeDetailsPage_ with the _Data_ as its recipe. This page can now display all the details of the recipe in question.

```xml
<uer:FeedView x:Name="TrendingNowFeed"
              Source="{Binding TrendingNow}">
    <DataTemplate>
        <ScrollViewer>
            <muxc:ItemsRepeater ItemsSource="{Binding Data}"
                                uen:Navigation.Request="RecipeDetails"
                                uen:Navigation.Data="{Binding Data}"
                                ItemTemplate="{StaticResource HomeLargeItemTemplate}">
                <muxc:ItemsRepeater.Layout>
                    ...
                </muxc:ItemsRepeater.Layout>
            </muxc:ItemsRepeater>
        </ScrollViewer>
    </DataTemplate>
</uer:FeedView>
```

## Source Code

- [ItemsRepeater navigation request](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Views/HomePage.xaml#L115-L135)
- [Back button](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Views/ReviewsPage.xaml#L75-L77)
- [Recipe record](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Business/Models/Recipe.cs)
- [App.cs ViewMap setup](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/App.cs#L104)
- [RecipeDetailsModel constructor](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/RecipeDetailsModel.cs#L10-L19)

## Documentation

- [How-To: Navigate in Xaml](xref:Uno.Extensions.Navigation.HowToNavigateInXAML)
