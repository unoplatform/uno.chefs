---
uid: Uno.Recipes.Configuration
---

# How to manage writable configuration data for your application

## Problem

There is a need for a uniform way to read configuration data as well as write configuration values at runtime.

## Solution

Writable configuration from `Uno.Extensions.Configuration` provides you an interface for using and updating local persistent data.

### Create a new record

```csharp
public record AppConfig
{
    public string? Title { get; init; }
    public bool? IsDark { get; init; }
    public bool? Notification { get; init; }
    public string? AccentColor { get; init; }
}
```

### Add to your `IConfigBuilder`

Use `Section<T>()` inside `UseConfiguration`. You can chain multiple configs.

```csharp
.UseConfiguration(configure: configBuilder =>
    configBuilder
        .EmbeddedSource<App>()
        .Section<AppConfig>()
        .Section<Credentials>()
        .Section<SearchHistory>()
)
```

### Get and Update the value

#### UserService.cs

1. Inject `IWritableOptions<AppConfig>` in the constructor.

```csharp
public class UserService(
    ChefsApiClient client,
    IWritableOptions<AppConfig> chefAppOptions,
    IWritableOptions<Credentials> credentialOptions)
    : IUserService
```

2. Implement the logic to read and write to the configuration.

```csharp
public async ValueTask<AppConfig> GetSettings(CancellationToken ct)
    => chefAppOptions.Value;
```

```csharp
 public async Task SetSettings(AppConfig chefSettings, CancellationToken ct)
 {
    var settings = new AppConfig
    {
    Title = chefSettings.Title,
    IsDark = chefSettings.IsDark,
    Notification = chefSettings.Notification,
    AccentColor = chefSettings.AccentColor,
    };

    await chefAppOptions.UpdateAsync(_ => settings);
 }
```

## Source Code

Chefs app

- [App UseConfiguration](https://github.com/unoplatform/uno.chefs/blob/04a93886dd0b530386997179b80453a59e832fbe/Chefs/App.xaml.host.cs#L65-L71)
- [App UserService](https://github.com/unoplatform/uno.chefs/blob/04a93886dd0b530386997179b80453a59e832fbe/Chefs/App.xaml.cs#L68)
- [UserService](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Services/Users/UserService.cs)
- [SettingModel](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/SettingsModel.cs#L22-L34)
- [SettingPage](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/SettingsPage.xaml#L120)

## Documentation

- [Uno.Extensions Configuration Documentation](xref:Uno.Extensions.Configuration.Overview)