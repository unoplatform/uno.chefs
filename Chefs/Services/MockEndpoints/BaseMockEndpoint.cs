namespace Chefs.Services.MockEndpoints;

public abstract class BaseMockEndpoint(ISerializer serializer)
{
	protected T? LoadData<T>(string fileName)
		=> serializer.FromString<T>(
			File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "AppData", fileName))
		);
}
