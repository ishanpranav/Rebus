// Ishan Pranav's REBUS: CultureInfoConverter.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

namespace System.Text.Json.Serialization.Converters
{
    /// <summary>
    /// Converts a <see cref="CultureInfo"/> value to or from JSON.
    /// </summary>
    public class CultureInfoConverter : JsonConverter<CultureInfo>
    {
        /// <inheritdoc/>
        public override CultureInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? text = reader.GetString();

            if (text == null)
            {
                return null;
            }
            else
            {
                return new CultureInfo(text);
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, CultureInfo? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Name);
            }
        }
    }
}
