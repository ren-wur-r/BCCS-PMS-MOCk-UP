using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommonLibrary.CmnJson;

public class CmnIpAddressJsonConverter : JsonConverter<IPAddress>
{
    public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (IPAddress.TryParse(reader.GetString(), out var ipAddress) == false)
        {
            throw new Exception("json converter read error");
        }

        return ipAddress;
    }

    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}
