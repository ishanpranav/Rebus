// Ishan Pranav's REBUS: QuadraticConsideration.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

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
