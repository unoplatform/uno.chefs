using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chefs.Models.Converters;
public class TimeSpanObjectConverter : JsonConverter<TimeSpan>
{
	public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return TimeSpan.Zero;
		}

		using var doc = JsonDocument.ParseValue(ref reader);
		var root = doc.RootElement;
		switch (root.ValueKind)
		{
			case JsonValueKind.String:
				return TimeSpan.Parse(root.GetString() ?? string.Empty);
			case JsonValueKind.Object when root.TryGetProperty("ticks", out var ticksElement):
				return new TimeSpan(ticksElement.GetInt64());
			default:
			{
				return new TimeSpan(root.GetProperty("ticks").GetInt64());
				}
		}
	}

	public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteNumber("ticks", value.Ticks);
		writer.WriteEndObject();
	}
}
