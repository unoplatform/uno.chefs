namespace Chefs.Business.Models;

public partial record Location
{
	internal Location(LocationData? location)
	{
		Id = location?.Id;
		Lat = location?.Lat;
		Lng = location?.Lng;
	}

	public int? Id { get; init; }
	public double? Lat { get; init; }
	public double? Lng { get; init; }

	internal LocationData ToData() => new()
	{
		Id = Id,
		Lat = Lat,
		Lng = Lng
	};
}
