---
uid: Uno.Recipes.KiotaMocking
---

# How to Mock Kiota API Clients

## Problem

You want to develop and test your Uno app using mock API data, without changing your generated Kiota API client or API code.

## Solution

The Chefs app uses the `USE_MOCKS` compile-time constant to control API mocking. When enabled, a custom `MockHttpMessageHandler` is registered as a `DelegatingHandler` in the `HttpClient` pipeline. This allows you to mock server responses at the HTTP level.

1. Enable Mocking

    Add a USE_MOCKS constant to your build constants and set it to `true`.

    ```xml
    <PropertyGroup Condition="'$(UseMocks)'=='true'">
        <DefineConstants>$(DefineConstants);USE_MOCKS</DefineConstants>
    </PropertyGroup>
    ```

2. Configure Kiota Client to Use the Mock Handler

    In `App.xaml.host.cs`, when USE_MOCKS is defined, configure the Kiota client to use your mock handler:

    ```csharp
    services.AddTransient<MockHttpMessageHandler>();
    services.AddKiotaClient<ChefsApiClient>(
        context,
        options: new EndpointOptions { Url = "http://localhost:5116" }
    #if USE_MOCKS
        , configure: (builder, endpoint) => builder.ConfigurePrimaryAndInnerHttpMessageHandler<MockHttpMessageHandler>()
    #endif
    );
    ```

3. Implement Your Mock Handler

    Your custom `MockHttpMessageHandler` inspects requests and returns mock responses based on local data, without ever hitting the real API server.

    ```csharp
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        // ...endpoint setup omitted for brevity...

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(await GetMockData(request), Encoding.UTF8, "application/json")
        };

        return await Task.FromResult(mockResponse);
        }

        private async Task<string> GetMockData(HttpRequestMessage request)
        {

            // Handle Notifications
            if (request.RequestUri.AbsolutePath.Contains("/api/Notification"))
            {
                return await _mockNotificationEndpoints.HandleNotificationsRequest(request);
            }
            ...

            return "{}";
        }
    }
    ```

4. Use the Kiota Client

    Consumers of the `ChefsApiClient` remain unchanged:

    ```csharp
    public class NotificationService(ChefsApiClient client) : INotificationService
        {
            public async ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct)
            {
                var notificationsData = await client.Api.Notification.GetAsync(cancellationToken: ct);
                return notificationsData?.Select(n => new Notification(n)).ToImmutableList() ?? ImmutableList<Notification>.Empty;
            }
        }
    ```

## Why This Pattern?

- No changes to Kiota-generated code.

- Mocking is controlled by a build constant, making it easy to switch between real and mock APIs.

- The rest of the app interacts with the API exactly as it would in production.

## Source Code

- [MockHttpMessageHandler](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/LoginPage.xaml#L41)
- [App.xaml.host.cs (Kiota client registration)](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/Chefs/App.xaml.host.cs#L26-L36)
- [NotificationService.cs](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/Chefs/Services/Notifications/NotificationService.cs)

## Documentation

- [Kiota documentation](xref:Uno.Extensions.Http.HowToKiota)