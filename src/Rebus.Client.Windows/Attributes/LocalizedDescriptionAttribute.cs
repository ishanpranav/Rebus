// Ishan Pranav's REBUS: LocalizedDescriptionAttribute.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;
using System.Resources;

namespace Rebus.Client.Windows.Attributes
{
    internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private static readonly ResourceManager s_resourceManager = new(typeof(LocalizedDescriptionAttribute));

        public LocalizedDescriptionAttribute(string description) : base(description)
        {
            DescriptionValue = s_resourceManager.GetString(description) ?? description;
        }
    }
}
