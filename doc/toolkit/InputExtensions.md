---
uid: Uno.Recipes.InputExtensions
---

# How to manage focus between input controls using the Enter key in XAML

## Problem

There is no easy way to denote the next control to receive focus when the Enter key is pressed while entering input in a control such as a `TextBox` or `PasswordBox`.

## Solution

The `InputExtensions` class provides a set of attached properties that can be used to manage focus between input controls using the Enter key.

```xml
...

<TextBox PlaceholderText="Username"
         utu:InputExtensions.AutoFocusNextElement="{Binding ElementName=EmailTxt}"
         ... />

<TextBox x:Name="EmailTxt"
         PlaceholderText="Email"
         utu:InputExtensions.AutoFocusNextElement="{Binding ElementName=PasswordTxt}"
         ... />

<PasswordBox x:Name="PasswordTxt"
             PlaceholderText="Password"
             utu:InputExtensions.AutoDismiss="True"
             ... />

...
```

The above code has the following effect:
<table>
  <tr>
    <th>InputExtensions.AutoFocusNextElement</th>
  </tr>
  <tr>
   <td><img src="../assets/inputextensions-animated.gif" width="400px" alt="InputExtensions Animation"/></td>
  </tr>
</table>

### Source Code

Chefs app

- [Login Page (1)](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/LoginPage.xaml#L64)
- [Login Page (2)](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/LoginPage.xaml#L167)
- [Login Page (3)](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/LoginPage.xaml#L186)

### Documentation

- [InputExtensions documentation](xref:Toolkit.Helpers.InputExtensions)