---
uid: Uno.Recipes.Serialization
---

# Serialization

## Problem

Accessing the serialized and deserialized representation of an object can be important for dynamic, data-rich applications. Currently, there is no shared abstract contract between serializers for different data formats (JSON, XML, etc.). As such, it can be difficult to inject an abstraction of a serializer object as a dependency into your view models or services.

## Solution

The Uno.Extensions library provides a set of abstractions for serialization and deserialization. This allows you to inject a serializer object as a dependency into your view models or services. The library includes implementations for JSON serialization as well as extension points for creating custom serializers.

### App Startup Configuration

```csharp
public partial class App : Application
{
    private void ConfigureAppBuilder(IApplicationBuilder builder)
    {
      builder
        .Configure(host => host
          // Code omitted for brevity

          // Register Json serializers (ISerializer and ISerializer)
          .UseSerialization()

          // Code omitted for brevity
        );
    }
}

### Consume `ISerializer` as dependency in data services

```csharp
public class MockNotificationEndpoints(string basePath, ISerializer serializer) : BaseMockEndpoint
{
  public string HandleNotificationsRequest(HttpRequestMessage request)
  {
    var notificationsData = LoadData("Notifications.json");
    var notifications = serializer.FromString<List<NotificationData>>(notificationsData);
  
    //Get all notifications
    if (request.RequestUri.AbsolutePath == "/api/notification" && request.Method == HttpMethod.Get)
    {
    return serializer.ToString(notifications);
    }
  
    return "{}";
  }
}
```

### Notification JSON data (notifications.json)

```json
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

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/04a93886dd0b530386997179b80453a59e832fbe/Chefs/App.xaml.host.cs#L77)
- [Notification Data Service](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Services/MockEndpoints/MockNotificationEndpoints.cs#L5)
- [Notification Data Model](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Data/Entities/NotificationData.cs)
- [JSON Data Files](https://github.com/unoplatform/uno.chefs/tree/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Data/AppData)

## Documentation

- [Uno.Extensions Serialization documentation](xref:Uno.Extensions.Serialization.Overview)
