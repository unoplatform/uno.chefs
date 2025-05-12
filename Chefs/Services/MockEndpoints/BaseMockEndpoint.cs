namespace Chefs.Services.MockEndpoints;

public abstract class BaseMockEndpoint
{
	private readonly ISerializer _serializer;
	protected BaseMockEndpoint(ISerializer serializer) => _serializer = serializer;

	protected T? LoadData<T>(string fileName)
		=> _serializer.FromString<T>(
			File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Data/AppData", fileName))
		);
}
