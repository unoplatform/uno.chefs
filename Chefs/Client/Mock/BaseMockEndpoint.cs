namespace Chefs.Client.Mock;

public abstract class BaseMockEndpoint(ISerializer serializer, ILogger<BaseMockEndpoint> logger)
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
			logger.LogError(ex, "Failed to load {FileName}", fileName);
			return default;
		}
	}
}
