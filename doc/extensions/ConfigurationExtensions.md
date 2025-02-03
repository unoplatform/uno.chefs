---
uid: Uno.Recipes.Configuration
---

# How to read/write configuration data for your application

## Problem

There is a need for a uniform way to read configuration data as well as write configuration values at runtime.

## Solution

Writable configuration from `Uno.Extensions.Configuration` provides you an interface for using and updating local persistent data.

### Create a new record

:::code language="csharp" source="../../src/Chefs/Business/Models/AppConfig.cs":::

### Add to your `IConfigBuilder`

Use `Section<T>()` inside `UseConfiguration`. You can chain multiple configs.

:::code language="csharp" source="../../src/Chefs/App.xaml.cs" range="62-68":::

### Get and Update the value

#### UserService.cs

1. Inject `IWritableOptions<AppConfig>` in the constructor.
:::code language="csharp" source="../../src/Chefs/Services/Users/UserService.cs" range="10-14":::
:::code language="csharp" source="../../src/Chefs/Services/Users/UserService.cs" range="6":::

2. Implement the logic to read and write to the configuration.
:::code language="csharp" source="../../src/Chefs/Services/Users/UserService.cs" range="20-52":::

## Source Code

Chefs app
- [App UseConfiguration](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/App.cs#L31)
- [App UserService](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/App.cs#L83)
- [UserService](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Services/Users/UserService.cs)
- [SettingModel](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Presentation/SettingsModel.cs#L22)
- [SettingPage](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/SettingsPage.xaml#L125)

## Documentation

- [Uno.Extensions Configuration Documentation](xref:Uno.Extensions.Configuration.Overview)