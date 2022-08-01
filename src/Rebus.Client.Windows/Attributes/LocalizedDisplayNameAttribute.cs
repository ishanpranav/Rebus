// Ishan Pranav's REBUS: LocalizedDisplayNameAttribute.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;
using System.Resources;

namespace Rebus.Client.Windows.Attributes
{
    internal sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private static readonly ResourceManager s_resourceManager = new(typeof(LocalizedDisplayNameAttribute));

        public LocalizedDisplayNameAttribute(string displayName) : base(displayName)
        {
            DisplayNameValue = s_resourceManager.GetString(displayName) ?? displayName;
        }
    }
}
