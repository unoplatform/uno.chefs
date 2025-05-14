using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chefs.Converters;

public class TimeSpanObjectConverter : JsonConverter<TimeSpanObject>
{
	public override TimeSpanObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return new TimeSpanObject();
		}

		if (reader.TokenType == JsonTokenType.String)
		{
			var s = reader.GetString();
			if (TimeSpan.TryParse(s, out var ts))
			{
				return new TimeSpanObject { Ticks = ts.Ticks };
			}

			throw new JsonException($"Cannot parse \"{s}\" as a TimeSpanObject");
		}

		using var doc = JsonDocument.ParseValue(ref reader);
		var root = doc.RootElement;
		if (root.ValueKind == JsonValueKind.Object
			&& root.TryGetProperty("ticks", out var tickElem))
		{
			return new TimeSpanObject { Ticks = tickElem.GetInt64() };
		}

		throw new JsonException($"Cannot parse {root} as a TimeSpanObject");
	}

	public override void Write(Utf8JsonWriter writer, TimeSpanObject value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteNumber("ticks", value.Ticks ?? 0);
		writer.WriteEndObject();
	}
}
