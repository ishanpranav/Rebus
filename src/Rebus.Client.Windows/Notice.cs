// Ishan Pranav's REBUS: License.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Client.Windows
{
    internal sealed class Notice
    {
        public string Product { get; }
        public string Title { get; }
        public string Body { get; }

        public Notice(string product, string title, string body)
        {
            Product = product;
            Title = title;
            Body = body;
        }

        public override string ToString()
        {
            return Product;
        }
    }
}
