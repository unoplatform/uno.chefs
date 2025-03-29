---
uid: Uno.Recipes.Serialization
---

# How to inject serializer objects as dependencies into your application

## Problem

Accessing the serialized and deserialized representation of an object can be important for dynamic, data-rich applications. Currently, there is no shared abstract contract between serializers for different data formats (JSON, XML, etc.). As such, it can be difficult to inject an abstraction of a serializer object as a dependency into your view models or services.

## Solution

The Uno.Extensions library provides a set of abstractions for serialization and deserialization. This allows you to inject a serializer object as a dependency into your view models or services. The library includes implementations for JSON serialization as well as extension points for creating custom serializers.

### App Startup Configuration

```csharp
public class App : Application
{
  // Code omitted for brevity

  protected async override void OnLaunched(LaunchActivatedEventArgs args)
  {
    var builder = this.CreateBuilder(args)
      .Configure(host => host
        // Code omitted for brevity
        
        // Register Json serializers (ISerializer and ISerializer)
        .UseSerialization()

        // Code omitted for brevity
    );
  
  // Code omitted for brevity
  }
}
```

### Consume `ISerializer` as dependency in data services

```csharp
public class NotificationEndpoint : INotificationEndpoint
{
  private readonly IStorage _dataService;
  private readonly ISerializer _serializer;
  private List<NotificationData>? _notifications;

  public NotificationEndpoint(IStorage dataService, ISerializer serializer)
    => (_dataService, _serializer) = (dataService, serializer);

  public async ValueTask<IImmutableList<NotificationData>> GetAll(CancellationToken ct)
    => (await Load(ct)).ToImmutableList() ?? ImmutableList<NotificationData>.Empty;

  private async ValueTask<IList<NotificationData>> Load(CancellationToken ct)
  {
    if (_notifications == null)
    {
      _notifications = await _dataService.ReadPackageFileAsync<List<NotificationData>>(_serializer, "notifications.json")
    }

    return _notifications ?? new List<NotificationData>();
  }
}
```

### Notification JSON data (notifications.json)

```json
﻿[
  {
    "Title": "New recipe!",
    "Description": "Far far away, behind the word mountains, far from the countries.",
    "Read": true,
    "Date": "2022-10-18T00:00:00Z"
  },
  {
    "Title": "Don’t forget to try your saved recipe",
    "Description": "Far far away, behind the word mountains, far from the countries.",
    "Read": true,
    "Date": "2022-10-18T00:00:00Z"
  },

  ...
]
```

### NotificationData model object

```csharp
public class NotificationData
{
  public string? Title { get; set; }
  public string? Description { get; set; }
  public bool Read { get; set; }
  public DateTime Date { get; set; }
}
```

## Source Code

Chefs app

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/App.cs#L43)
- [Notification Data Service](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Services/Endpoints/NotificationEndpoint.cs#L6)
- [Notification Data Model](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Data/Entities/NotificationData.cs)
- [JSON Data Files](https://github.com/unoplatform/uno.chefs/tree/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Data/AppData)

## Documentation

- [Uno.Extensions Serialization documentation](xref:Uno.Extensions.Serialization.Overview)
