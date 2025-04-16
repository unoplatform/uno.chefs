---
uid: Uno.Recipes.Configuration
---

# How to read/write configuration data for your application

## Problem

There is a need for a uniform way to read configuration data as well as write configuration values at runtime.

## Solution

Writable configuration from `Uno.Extensions.Configuration` provides you an interface for using and updating local persistent data.

### Create a new record

[!code-csharp[](../../Chefs/Business/Models/AppConfig.cs#L3-L9)]

### Add to your `IConfigBuilder`

Use `Section<T>()` inside `UseConfiguration`. You can chain multiple configs.

[!code-csharp[](../../Chefs/App.xaml.cs#L82-L88)]

### Get and Update the value

#### UserService.cs

1. Inject `IWritableOptions<AppConfig>` in the constructor.

[!code-csharp[](../../Chefs/Services/Users/UserService.cs#L7-L11)]

2. Implement the logic to read and write to the configuration.

[!code-csharp[](../../Chefs/Services/Users/UserService.cs#L19-L20)]

[!code-csharp[](../../Chefs/Services/Users/UserService.cs#L38-L49)]

## Source Code

Chefs app
- [App UseConfiguration](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L82-L88)
- [App UserService](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L155)
- [UserService](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Services/Users/UserService.cs)
- [SettingModel](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/SettingsModel.cs#L22-L34)
- [SettingPage](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/SettingsPage.xaml#L120)

## Documentation

- [Uno.Extensions Configuration Documentation](xref:Uno.Extensions.Configuration.Overview)