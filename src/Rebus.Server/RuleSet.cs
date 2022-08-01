// Ishan Pranav's REBUS: RuleSet.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Server
{
    internal sealed class RuleSet
    {
        public int FuelCost { get; }
        public int MinUnitsCaptured { get; }
        public double UnitCaptureRate { get; }

        public RuleSet(int fuelCost, int minUnitsCaptured, double unitCaptureRate)
        {
            FuelCost = fuelCost;
            MinUnitsCaptured = minUnitsCaptured;
            UnitCaptureRate = unitCaptureRate;
        }
    }
}
