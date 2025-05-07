using System.Reflection;

namespace Chefs.DataContracts;

public static class EmbeddedJsonLoader
{
	private static readonly Assembly assembly = typeof(EmbeddedJsonLoader).Assembly;


	public static string Load(string fileName)
	{
		var resourceName = assembly
			.GetManifestResourceNames()
			.FirstOrDefault(r => r.EndsWith($".AppData.{fileName}", StringComparison.OrdinalIgnoreCase));

		if (resourceName is null)
			throw new InvalidOperationException($"Embedded resource '{fileName}' not found in {nameof(Chefs.DataContracts)}.");

		using var stream = assembly.GetManifestResourceStream(resourceName)!;
		using var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}
}
