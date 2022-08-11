// Ishan Pranav's REBUS: 
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server
{
    public class SelectionContext
    {
        private readonly Dictionary<string, double> _values = new Dictionary<string, double>();

        public double X { get; set; }
        
        public double this[string key]
        {
            get
            {
                return X;
            }
        }
    }
}
