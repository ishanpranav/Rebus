// Ishan Pranav's REBUS: ExponentialDistribution.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.Server
{
    internal sealed class ExponentialDistribution
    {
        private readonly Random _random;
        private readonly double _lambda;

        public ExponentialDistribution(Random random, double lambda)
        {
            _random = random;
            _lambda = lambda;
        }

        public double NextDouble()
        {
            return -Math.Log(1 - (1 - Math.Exp(-_lambda)) * _random.NextDouble()) / _lambda;
        }

        public int Next(int minValue, int maxValue)
        {
            return (int)((NextDouble() * (maxValue - minValue)) + minValue);
        }
    }
}
