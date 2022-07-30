// Ishan Pranav's REBUS: 
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Server
{
    public class RuleSet
    {
        public int FuelCost { get; }
        public int MinUnitsCaptured { get; }

        public RuleSet(int fuelCost, int minUnitsCaptured)
        {
            FuelCost = fuelCost;
            MinUnitsCaptured = minUnitsCaptured;
        }
    }
}
