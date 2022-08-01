// Ishan Pranav's REBUS: StringCollectionLoader.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;

namespace Rebus.Server
{
    internal sealed class StringCollectionLoader
    {
        private readonly string _connectionString;

        public StringCollectionLoader(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<string> Load(string key)
        {
            List<string> results = new List<string>();

            using (TextReader reader = File.OpenText(Path.ChangeExtension(Path.Combine(_connectionString, key), extension: "txt")))
            {
                while (reader.Peek() != -1)
                {
                    string? line = reader
                        .ReadLine()?
                        .Trim();

                    if (!string.IsNullOrWhiteSpace(line) && line[0] != '#')
                    {
                        results.Add(line);
                    }
                }
            }

            return results;
        }
    }
}
