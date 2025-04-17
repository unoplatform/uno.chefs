using System.Text.Json.Serialization;


namespace Chefs.Business.Models;

[JsonSerializable(typeof(AppConfig))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(Credentials))]
[JsonSerializable(typeof(SearchHistory))]
[JsonSerializable(typeof(Dictionary<string, AppConfig>))]
[JsonSerializable(typeof(Dictionary<string, User>))]
[JsonSerializable(typeof(Dictionary<string, Credentials>))]
[JsonSerializable(typeof(Dictionary<string, SearchHistory>))]
public partial class ModelSerializerContext : JsonSerializerContext
{
}
