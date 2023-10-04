using Mapsui.Tiling;

namespace Chefs.Services.Map;

public class MapService : IMapService
{
	public async ValueTask<Mapsui.Map> GetCurrentMapAsync(CancellationToken ct)
	{
		var map = new Mapsui.Map();

		map.Layers.Add(OpenStreetMap.CreateTileLayer());
		return map;
	}
}
