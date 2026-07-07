using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hikari.Common.Text.Json.JsonConverter
{
    /// <summary>
    /// 灵活字符串转换器
    /// </summary>
    /// <remarks>options.JsonSerializerOptions.Converters.Add(new FlexibleStringConverter());</remarks>
    public class FlexibleStringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.TryGetInt64(out long l)
                    ? l.ToString()
                    : reader.GetDouble().ToString(System.Globalization.CultureInfo.InvariantCulture),
                JsonTokenType.Null => null,
                _ => throw new JsonException($"Unexpected token type: {reader.TokenType} when reading string.")
            };
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
