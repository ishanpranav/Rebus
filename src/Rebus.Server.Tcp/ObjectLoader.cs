// Ishan Pranav's REBUS: ObjectSaver.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text.Json;

namespace Rebus.Server.Tcp
{
    internal sealed class ObjectLoader
    {
        private readonly string _connectionString;
        private readonly JsonSerializerOptions _options;

        public ObjectLoader(string connectionString, JsonSerializerOptions options)
        {
            _connectionString = connectionString;
            _options = options;
        }

        public T Load<T>()
        {
            using (FileStream utf8Json = File.OpenRead(Path.ChangeExtension(Path.Combine(_connectionString, JsonNamingPolicy.CamelCase.ConvertName(typeof(T).Name)), extension: "json")))
            {
                T? result = JsonSerializer.Deserialize<T>(utf8Json, _options);

                if (result == null)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return result;
                }
            }
        }
    }
}
