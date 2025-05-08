---
uid: Uno.Recipes.InputExtensions.Focus
---

# Navigating Between Input Controls with the Keyboard

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

<img src="../assets/inputextensions-animated.gif" alt="InputExtensions Animation" width="300" />

## Source Code

- [Registration Page (1)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RegistrationPage.xaml#L31)
- [Registration Page (2)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RegistrationPage.xaml#L43)

## Documentation

- [InputExtensions documentation](xref:Toolkit.Helpers.InputExtensions)
