// Ishan Pranav's REBUS: JsonComplexConverter.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rebus.Server
{
    internal sealed class JsonComplexConverter : JsonConverter<Complex>
    {
        public override Complex Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? text = reader.GetString();
            double real = 0;
            double imaginary = 0;

            if (text != null)
            {
                string[] components = text.Split('+');

                foreach (string component in components)
                {
                    if (component.EndsWith('i'))
                    {
                        imaginary += double.Parse(component.Substring(startIndex: 0, component.Length - 1));
                    }
                    else
                    {
                        real += double.Parse(component);
                    }
                }
            }

            return new Complex(real, imaginary);
        }

        public override void Write(Utf8JsonWriter writer, Complex value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value.Real} + {value.Imaginary}i");
        }
    }
}
