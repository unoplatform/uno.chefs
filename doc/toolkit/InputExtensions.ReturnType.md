---
uid: Uno.Recipes.InputExtensions.ReturnType
---

# How to Use the InputExtensions.ReturnType Property

## Problem

There is no cross-platform method to set the on-screen keyboard's return key for input controls like `TextBox` and `PasswordBox`. Normally, you would need to use platform conditionals to set the `ImeOptions` or the `ReturnKeyType` properties for Android and iOS, respectively.

## Solution

The `InputExtensions` class, provided by **Uno.Toolkit**, includes the `InputExtensions.ReturnType` property which simplifies setting the return type for input controls, ensuring a consistent user experience across all platforms.

In the Chefs app, the `RegistrationPage` sets the return type for `TextBox` and `PasswordBox` controls without needing platform specific properties:

```xml
<TextBox PlaceholderText="Username"
         utu:InputExtensions.ReturnType="Next" />

<TextBox x:Name="RegistrationEmail"
         PlaceholderText="Email"
         utu:InputExtensions.ReturnType="Next" />

<PasswordBox x:Name="RegistrationPassword"
             PlaceholderText="Password"
             utu:InputExtensions.ReturnType="Done" />
```

## Source Code

- [Registration Page (1)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RegistrationPage.xaml#L30)
- [Registration Page (2)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RegistrationPage.xaml#L42)
- [Registration Page (3)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RegistrationPage.xaml#L50)

## Documentation

- [InputExtensions documentation](xref:Toolkit.Helpers.InputExtensions)
