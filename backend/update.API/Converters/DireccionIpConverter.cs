using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace update.API.Converters;

/// <summary>Representa inet como una dirección textual IPv4 o IPv6 en los DTOs JSON.</summary>
public sealed class DireccionIpConverter : JsonConverter<IPAddress>
{
    public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String || !IPAddress.TryParse(reader.GetString(), out var direccion))
            throw new JsonException("Se requiere una dirección IP válida.");
        return direccion;
    }
    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}
