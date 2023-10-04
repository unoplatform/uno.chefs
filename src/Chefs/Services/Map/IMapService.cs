namespace Chefs.Services.Map;

/// <summary>
/// Implements map related methods
/// </summary>
public interface IMapService
{
	ValueTask<Mapsui.Map> GetCurrentMapAsync(CancellationToken ct);
}
