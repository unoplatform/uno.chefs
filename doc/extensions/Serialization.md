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

[!code-csharp[](../../Chefs/Services/MockEndpoints/MockNotificationEndpoints.cs#L5-L20)]

### Notification JSON data (notifications.json)

[!code-json[](../../Chefs/Data/AppData/Notifications.json#L2-L13)]

### NotificationData model object

[!code-csharp[](../../Chefs/Data/Entities/NotificationData.cs#L3-L9)]

## Source Code

Chefs app

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L94)
- [Notification Data Service](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Services/MockEndpoints/MockNotificationEndpoints.cs#L5)
- [Notification Data Model](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Data/Entities/NotificationData.cs)
- [JSON Data Files](https://github.com/unoplatform/uno.chefs/tree/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Data/AppData)

## Documentation

- [Uno.Extensions Serialization documentation](xref:Uno.Extensions.Serialization.Overview)
