// Ishan Pranav's REBUS: Consideration.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Server.Considerations
{
    public abstract class Consideration
    {
        public abstract double Evaluate(EvaluationContext context);
    }
}
