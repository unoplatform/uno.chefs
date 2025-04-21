---
uid: Uno.Recipes.SafeArea
---

# How to restrict page content to the Safe Area on mobile devices

## Problem

In a mobile platform context, UI elements can be obscured by device-specific features like status bars, rounded corners, notches, or soft-input panels like on-screen keyboards. Certain content, such as readable text or interactive controls, needs to stay within the visible screen area to ensure usability.

## Solution

[!code-xml[](../../Chefs/Views/WelcomePage.xaml#L21)]

Without SafeArea|SafeArea applied
-|-
![SafeArea not implemented](../assets/without-safearea.png)|![SafeArea implemented](../assets/with-safearea.png)

## Source Code

- [Welcome Page](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/WelcomePage.xaml#L21)

## Documentation

- [SafeArea documentation](xref:Toolkit.Controls.SafeArea)
