// Ishan Pranav's REBUS: 
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml.Serialization;

namespace Rebus.Server.Considerations
{
    public abstract class Consideration
    {
        public abstract double Evaluate(SelectionContext context);
    }
}
