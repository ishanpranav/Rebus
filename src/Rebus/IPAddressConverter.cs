using System.Net;

namespace System.Text.Json.Serialization.Converters
{
    /// <summary>
    /// Converts an <see cref="IPAddress"/> value to or from JSON.
    /// </summary>
    public class IPAddressConverter : JsonConverter<IPAddress>
    {
        /// <inheritdoc/>
        public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? text = reader.GetString();

            if (text == null)
            {
                return null;
            }
            else
            {
                return IPAddress.Parse(text);
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IPAddress? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
