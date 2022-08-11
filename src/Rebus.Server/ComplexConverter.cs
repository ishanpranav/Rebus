// Ishan Pranav's REBUS: ComplexConverter.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Numerics;

namespace System.Text.Json.Serialization.Converters
{
    /// <summary>
    /// Converts a <see cref="Complex"/> value to or from JSON.
    /// </summary>
    public class ComplexConverter : JsonConverter<Complex>
    {
        /// <inheritdoc/>
        public override Complex Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            double real = 0;
            double imaginary = 0;

            while (reader.TokenType != JsonTokenType.EndObject)
            {
                reader.Read();

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string? text = reader.GetString();

                    if (text != null)
                    {
                        reader.Read();

                        StringComparison comparisonType;

                        if (options.PropertyNameCaseInsensitive)
                        {
                            comparisonType = StringComparison.OrdinalIgnoreCase;
                        }
                        else
                        {
                            comparisonType = StringComparison.Ordinal;
                        }

                        if (text.Equals(GetPropertyName(nameof(Complex.Real), options.PropertyNamingPolicy), comparisonType))
                        {
                            real = reader.GetDouble();
                        }
                        else if (text.Equals(GetPropertyName(nameof(Complex.Imaginary), options.PropertyNamingPolicy), comparisonType))
                        {
                            imaginary = reader.GetDouble();
                        }
                    }
                }
            }

            return new Complex(real, imaginary);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, Complex value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber(GetPropertyName(nameof(Complex.Real), options.PropertyNamingPolicy), value.Real);
            writer.WriteNumber(GetPropertyName(nameof(Complex.Imaginary), options.PropertyNamingPolicy), value.Imaginary);
            writer.WriteEndObject();
        }

        private static string GetPropertyName(string propertyName, JsonNamingPolicy? namingPolicy)
        {
            if (namingPolicy == null)
            {
                return propertyName;
            }
            else
            {
                return namingPolicy.ConvertName(propertyName);
            }
        }
    }
}
