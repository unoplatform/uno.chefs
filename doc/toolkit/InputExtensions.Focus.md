---
uid: Uno.Recipes.InputExtensions.Focus
---

# How to manage focus between input controls using the Enter key in XAML

## Problem

There is no easy way to denote the next control to receive focus when the Enter key is pressed while entering input in a control such as a `TextBox` or `PasswordBox`.

## Solution

The `InputExtensions` class provides a set of attached properties that can be used to manage focus between input controls using the Enter key.

```xml
<TextBox PlaceholderText="Username"
         utu:InputExtensions.AutoFocusNextElement="{Binding ElementName=RegistrationEmail}" />

<TextBox x:Name="RegistrationEmail"
         PlaceholderText="Email"
         utu:InputExtensions.AutoFocusNextElement="{Binding ElementName=RegistrationPassword}" />

<PasswordBox x:Name="RegistrationPassword"
             PlaceholderText="Password" />
```

The above code has the following effect:

![InputExtensions Animation](../assets/inputextensions-animated.gif)

## Source Code

- [Registration Page (1)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/RegistrationPage.xaml#L31)
- [Registration Page (2)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/RegistrationPage.xaml#L43)

## Documentation

- [InputExtensions documentation](xref:Toolkit.Helpers.InputExtensions)
