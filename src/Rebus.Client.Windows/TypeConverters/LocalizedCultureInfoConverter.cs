// Ishan Pranav's REBUS: LocalizedCultureInfoConverter.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace Rebus.Client.Windows.TypeConverters
{
    internal sealed class LocalizedCultureInfoConverter : CultureInfoConverter
    {
        private readonly StandardValuesCollection _standardValues;

        public LocalizedCultureInfoConverter()
        {
            List<CultureInfo?> cultures = new List<CultureInfo?>()
            {
                null,
                new CultureInfo(name: "en-US")
            };

            foreach (string directory in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                string name = new DirectoryInfo(directory).Name;

                if (!name.Equals(value: "ref", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        cultures.Add(new CultureInfo(name));
                    }
                    catch (CultureNotFoundException)
                    {

                    }
                }
            }

            cultures.Sort((x, y) =>
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (y == null)
                {
                    return 1;
                }
                else
                {
                    return x.DisplayName.CompareTo(y.DisplayName);
                }
            });

            _standardValues = new StandardValuesCollection(cultures);
        }

        protected override string GetCultureName(CultureInfo culture)
        {
            return culture.DisplayName;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return _standardValues;
        }
    }
}
