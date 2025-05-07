using Chefs.DataContracts;

namespace Chefs.Services;

public abstract class BaseMockEndpoint
{
	public string LoadData(string fileName)
	{
		return EmbeddedJsonLoader.Load(fileName);
	}
}
