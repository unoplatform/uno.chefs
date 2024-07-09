---
uid: Uno.Recipes.InputExtensions.ReturnType
---

# How to Use the InputExtensions.ReturnType Property

## Problem

There is no easy way to set the return type for input controls like `TextBox` and `PasswordBox` to control the keyboard behavior can often be inconsistent and verbose across different platforms.

## Solution

The `InputExtensions` class provides the `InputExtensions.ReturnType` property provided by **Uno.Toolkit** simplifies setting the return type for input controls, ensuring a consistent user experience across all platforms.


In the Chefs app, we can use `InputExtensions.ReturnType` to set the return type.In the `LoginPage`, we can set the return type for `TextBox` and `PasswordBox` controls without needing platform specific properties:

```xml
...

<TextBox PlaceholderText="Username"
         utu:InputExtensions.ReturnType="Next" />



<PasswordBox x:Name="LoginPassword"
             utu:InputExtensions.ReturnType="Done"/>
             
...
```

The above code has the following effect:
<table>
  <tr>
    <th>InputExtensions.AutoFocusNextElement</th>
  </tr>
  <tr>
     <td><img src="../assets/inputextensions-returnType-2.png" width="400px" alt="InputExtensions.ReturnType='Done'"/></td>
   <td><img src="../assets/inputextensions-returnType-1.png" width="400px" alt="InputExtensions.ReturnType='Done'"/></td>
  </tr>
</table>

## Source Code

Chefs app

- [Login Page](https://github.com/unoplatform/uno.chefs/blob/57492ecaf328df3437fe42777f1c085e6fda8212/src/Chefs/Views/LoginPage.xaml)
- [Registration Page](https://github.com/unoplatform/uno.chefs/blob/57492ecaf328df3437fe42777f1c085e6fda8212/src/Chefs/Views/RegistrationPage.xaml)

## Documentation

- [InputExtensions documentation](xref:Toolkit.Helpers.InputExtensions)