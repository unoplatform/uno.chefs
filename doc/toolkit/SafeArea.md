---
uid: Uno.Recipes.SafeArea
---

# How to restrict page content to the Safe Area on mobile devices

## Problem

In a mobile platform context, UI elements can be obscured by device-specific features like status bars, rounded corners, notches, or soft-input panels like on-screen keyboards. Certain content, such as readable text or interactive controls, needs to stay within the visible screen area to ensure usability.

## Solution

```xml
<Page ...
      xmlns:utu="using:Uno.Toolkit.UI"
      utu:SafeArea.Insets="VisibleBounds">

    ...

</Page>
```

Without SafeArea|SafeArea applied
-|-
![SafeArea not implemented](../assets/without-safearea.png)|![SafeArea implemented](../assets/with-safearea.png)

## Source Code

- [Welcome Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/WelcomePage.xaml#L17)

## Documentation

- [SafeArea documentation](xref:Toolkit.Controls.SafeArea)
