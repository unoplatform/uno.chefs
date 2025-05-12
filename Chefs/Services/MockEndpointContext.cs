using System.Text.Json.Serialization;
using Chefs.Services.Clients.Models;

namespace Chefs.Data;


[JsonSerializable(typeof(LoginRequest))]

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true)]
public partial class MockEndpointContext : JsonSerializerContext
{
}
