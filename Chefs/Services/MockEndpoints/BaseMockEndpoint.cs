namespace Chefs.Services.MockEndpoints;

public abstract class BaseMockEndpoint(ISerializer serializer)
{
	protected T? LoadData<T>(string fileName)
	{
		var assembly = this.GetType().Assembly;
		var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(str => str.EndsWith(fileName));
		if (resourceName == null)
		{
			throw new Exception($"Resource {fileName} not found in assembly {assembly.FullName}");
		}
		using var stream = assembly.GetManifestResourceStream(resourceName);
		using var reader = new System.IO.StreamReader(stream);
		return serializer.FromString<T>(reader.ReadToEnd());
	}
}
