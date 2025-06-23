---
uid: Uno.Recipes.ReactiveCarousel
---

# Carousel

## Problem

You need to display collections of content in an interactive, navigable format, similar to a photo carousel or image slider.

## Solution

The `FlipView` and `PipsPager` controls offer a robust and elegant solution for creating carousel-like experiences in your applications. The `FlipView` enables users to navigate through a collection of items, providing a fluid swiping or paging mechanism. The `PipsPager` acts as a visual indicator, displaying a series of dots or "pips" that represent the total number of items and highlighting the currently selected item.

By combining these two controls, you can create a photo carousel where navigating through the `FlipView` items automatically updates the `PipsPager` to show the current item's position, providing a clear and intuitive user experience. We will use the Uno Toolkit library, specifically the `SelectorExtensions` attached property, to connect the `FlipView` and the `PipsPage` with a single line of code.

```xml
<StackPanel Orientation="Vertical" Spacing="10" Margin="10">
    <FlipView utu:SelectorExtensions.PipsPager="{Binding ElementName=pipsPager}">
        <FlipView.Items>
            <!-- First Carrousel image -->
            <Image Source="https://picsum.photos/id/1015/1200/800"
                Stretch="UniformToFill" />

            <!-- Second Carrousel image -->
            <Image Source="https://picsum.photos/id/1016/1200/800"
                Stretch="UniformToFill" />

            <!-- Third Carrousel image -->
            <Image Source="https://picsum.photos/id/1018/1200/800"
                Stretch="UniformToFill" />
        </FlipView.Items>
    </FlipView>

    <muxc:PipsPager x:Name="pipsPager"
                    Margin="0,0,0,10"
                    HorizontalAlignment="Center"
                    Orientation="Horizontal" />
</StackPanel>
```

## Source Code

- [Welcome Page](https://github.com/unoplatform/uno.chefs/blob/main/Chefs/Views/WelcomePage.xaml#L46-L76)

## Documentation

- [SelectorExtensions](xref:Toolkit.Helpers.SelectorExtensions)
