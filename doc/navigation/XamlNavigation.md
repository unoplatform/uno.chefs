---
uid: Uno.Recipes.XamlNavigation
---

# How to Navigate with Xaml

## Problem

Navigating between pages often involves managing code-behind logic to handle different navigation methods as well as maintaining states, often with data that's already available in the view layer of the app. This can make the code feel bulkier than it needs to be, and it tightly couples the UI layer with navigation methods in the model.

## Solution

Uno Navigation Extensions allows you to streamline Xaml navigation by leveraging attached properties such as Navigation.Request and Navigation.Data. This approach simplifies the process of navigating and passing data between pages, allowing it to [all be done directly in Xaml](xref:Uno.Extensions.Navigation.HowToNavigateInXAML). In most cases it promotes better readability and separation of concerns by keeping everything navigation related in the view layer.

### Chefs Examples

#### General Usage

On the Chefs home page there are some `ItemRepeater` controls that each display a list of filtered clickable recipes. When a user clicks on a recipe, they are brought to that specific recipe's details page.

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

We can see that the `ItemsSource` is bound to the Data of the current context, which is from the `FeedView` control's `Source` _TrendingNow_ (of type `IListFeed<Recipe>`). Therefore, we will get a list of recipes, and clicking on a specific recipe will bring the user to the _RecipeDetailsPage_ with the _Data_ as its recipe. This page can now display all the details of the recipe in question. You can find more information about Navigation Requests [here](xref:Uno.Extensions.Navigation.HowToNavigateInXAML#1-navigationrequest).

#### Qualifiers

Navigation Extensions also provides multiple [qualifiers](xref:Reference.Navigation.Qualifiers) that you can add to the `Navigation.Request`. There's also more [advanced techniques](xref:Uno.Extensions.Navigation.Advanced.PageNavigation) you can put in place to better handle the navigation back-stack.

```xml
<Button ...
        uen:Navigation.Request="-/Login"
        Content="Skip" />
```

On the Welcome page in Chefs when allowing the user to skip some content, we are using the `-/` qualifier which clears the navigation back-stack while navigating to the Login page. When a user navigates to the Login page, they will not be able to return to the previous page through the back-stack because it will be empty.

## Source Code

- [ItemsRepeater navigation request](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Views/HomePage.xaml#L115-L135)
- [Skipping to the Login Page with the -/ Qualifier](https://github.com/unoplatform/uno.chefs/blob/40918c8347386e63237f5ff4c93a61e315fec7d1/src/Chefs/Views/WelcomePage.xaml#L77)

## Documentation

- [How-To: Navigate in Xaml](xref:Uno.Extensions.Navigation.HowToNavigateInXAML)
