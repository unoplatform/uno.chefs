using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chefs.Services;

public abstract class BaseMockEndpoint
{
	public string LoadData(string fileName)
	{
		var assembly = this.GetType().Assembly;
		var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(str => str.EndsWith(fileName));
		if (resourceName == null)
		{
			throw new Exception($"Resource {fileName} not found in assembly {assembly.FullName}");
		}
		using var stream = assembly.GetManifestResourceStream(resourceName);
		using var reader = new System.IO.StreamReader(stream);
		return reader.ReadToEnd();
	}
}
