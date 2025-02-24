using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chefs.Converters;
public class TimeSpanObjectConverter : JsonConverter<TimeSpan>
{
	public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return TimeSpan.Zero;
		}

		using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
		{
			var root = doc.RootElement;
			var ticks = root.GetProperty("ticks").GetInt64();
			return new TimeSpan(ticks);
		}
	}

	public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteNumber("ticks", value.Ticks);
		writer.WriteEndObject();
	}
}
