using System.Reflection;

namespace Chefs.Api.Controllers;

public abstract class ChefsControllerBase : ControllerBase
{
	//load data from embedded resource
	protected T LoadData<T>(string fileName)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourceName = $"Chefs.Api.AppData.{fileName}";
		using var stream = assembly.GetManifestResourceStream(resourceName);
		if (stream == null) throw new FileNotFoundException($"Resource '{resourceName}' not found.");
		using var reader = new StreamReader(stream);
		var json = reader.ReadToEnd();
		return JsonSerializer.Deserialize<T>(json) ?? throw new JsonException($"Failed to deserialize JSON from '{resourceName}'.");
	}
}
