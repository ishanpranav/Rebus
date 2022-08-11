// Ishan Pranav's REBUS: PolynomialConsideration.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml.Serialization;

namespace Rebus.Server.Considerations
{
    public class QuadraticConsideration : FunctionConsideration
    {
        protected override double Evaluate(double x)
        {
            return x * x;
        }
    }
}
