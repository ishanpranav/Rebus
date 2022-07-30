// Ishan Pranav's REBUS: Parser.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rebus.Client.Windows
{
    internal sealed class NoticeParser
    {
        public async IAsyncEnumerable<Notice> ParseAsync(TextReader reader)
        {
            int delimiters = 0;
            int prefixLength = 0;
            List<StringBuilder> headerBuilders = new List<StringBuilder>();
            StringBuilder bodyBuilder = new StringBuilder();

            while (reader.Peek() != -1)
            {
                string? line = await reader.ReadLineAsync();

                if (line != null)
                {
                    if (line.StartsWith(value: "----"))
                    {
                        delimiters++;

                        if (delimiters == 3)
                        {
                            foreach (Notice notice in createNotices())
                            {
                                yield return notice;
                            }

                            delimiters = 1;
                            prefixLength = 0;
                        }
                    }
                    else
                    {
                        switch (delimiters)
                        {
                            case 1:
                                if (equals(value: "References") || startsWith(prefix: "License notice for ") || startsWith(prefix: "Attribution for "))
                                {
                                    headerBuilders.Add(new StringBuilder(line));
                                }
                                else
                                {
                                    headerBuilders[headerBuilders.Count - 1].Append(line);
                                }
                                break;

                            case 2:
                                if (!string.IsNullOrWhiteSpace(line) || bodyBuilder.Length > 0)
                                {
                                    bodyBuilder.AppendLine(line);
                                }
                                break;
                        }
                    }
                }

                bool equals(string value)
                {
                    if (line.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        prefixLength = 0;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                bool startsWith(string prefix)
                {
                    if (line.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        prefixLength = prefix.Length;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (delimiters == 2)
            {
                foreach (Notice notice in createNotices())
                {
                    yield return notice;
                }
            }

            IEnumerable<Notice> createNotices()
            {
                string body = bodyBuilder.ToString();

                foreach (StringBuilder headerBuilder in headerBuilders)
                {
                    string header = headerBuilder
                        .ToString()
                        .Trim();
                    int lastIndex = header.LastIndexOf(value: '-');

                    if (lastIndex < 0)
                    {
                        lastIndex = header.Length;
                    }

                    yield return new Notice(header.Substring(prefixLength, lastIndex - prefixLength), header.Substring(startIndex: 0, lastIndex), body);
                }

                headerBuilders.Clear();
                bodyBuilder.Clear();
            }
        }
    }
}
