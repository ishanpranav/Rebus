// Ishan Pranav's REBUS: ObjectSaver.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rebus.Client.Windows
{
    internal sealed class ObjectSaver
    {
        private readonly JsonSerializerOptions _options;

        public ObjectSaver(JsonSerializerOptions options)
        {
            _options = options;
        }

        private static string GetPath(Type type)
        {
            return Path.ChangeExtension(Path.Combine(path1: "objects", JsonNamingPolicy.CamelCase.ConvertName(type.Name)), extension: "json");
        }

        public async Task SaveAsync<T>(T value) where T : notnull
        {
            try
            {
                using (FileStream utf8Json = File.Create(GetPath(value.GetType())))
                {
                    await JsonSerializer.SerializeAsync(utf8Json, value, _options);
                }
            }
            catch (IOException) { }
        }

        public T Load<T>() where T : new()
        {
            try
            {
                using (FileStream utf8Json = File.OpenRead(GetPath(typeof(T))))
                {
                    return JsonSerializer.Deserialize<T>(utf8Json, _options) ?? new T();
                }
            }
            catch
            {
                return new T();
            }
        }
    }
}
