---
uid: Uno.Recipes.StorageFile
---

# Loading App Data

## Problem

You need to load app data files on all platforms. Traditional file access methods (`System.IO.File.Read*`, `EmbeddedResource`) do not work for Content files on WASM.

## Solution

Use the `Windows.Storage.StorageFile` API to read files from your app package. This works the same way on all Uno Platform targets, including WASM.

- Move your data files to be included as `Content` in your project, and use `StorageFile.GetFileFromApplicationUriAsync` to load them by path.

    ```xml
    <ItemGroup>
        <Content Include="AppData\*.json" />
    </ItemGroup>
    ```

- Loading a JSON File Using StorageFile

    ```csharp
    public abstract class BaseMockEndpoint(ISerializer serializer, ILogger<BaseMockEndpoint> _logger)
    {
        protected async Task<T?> LoadData<T>(string fileName)
        {
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///AppData/{fileName}"));
                var json = await FileIO.ReadTextAsync(file);
                return serializer.FromString<T>(json);
            }
                catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load {FileName}", fileName);
                return default;
            }
        }
    }
    ```

- Using `LoadData` in `MockRecipeEndpoints`

    ```csharp
    public class MockRecipeEndpoints(string basePath, ISerializer serializer, ILogger<BaseMockEndpoint> logger) : BaseMockEndpoint(serializer, logger)
    {
        public async Task<string> HandleRecipesRequest(HttpRequestMessage request)
        {
            var savedList = await LoadData<List<Guid>>("SavedRecipes.json") ?? [];
            var allRecipes = await LoadData<List<RecipeData>>("Recipes.json") ?? [];
            ...
        }
    }
    ```

## Source Code

- [BaseMockEndpoint](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/Chefs/Services/MockEndpoints/BaseMockEndpoint.cs)
- [MockRecipeEndpoints](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/Chefs/Services/MockEndpoints/MockRecipeEndpoints.cs#L8)
- [Recipes.json](https://github.com/unoplatform/uno.chefs/blob/060fe206b949c23ca96ad15374a8b6b2d337bd33/AppData/Recipes.json)

## Documentation

- [File Management documentation](xref:Uno.Features.FileManagement)
