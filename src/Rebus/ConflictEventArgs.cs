// Ishan Pranav's REBUS: ConflictEventArgs.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using MessagePack;

namespace Rebus
{
    [MessagePackObject]
    public class ConflictEventArgs : System.EventArgs
    {
        [Key(0)]
        public HexPoint Location { get; }

        [Key(1)]
        public bool InvasionSucceeded { get; }

        [Key(2)]
        public Fleet Invader { get; }

        [Key(3)]
        public Fleet Occupant { get; }

        [Key(4)]
        public int UnitsRetreated { get; }

        [Key(5)]
        public int UnitsDestroyed { get; }

        [Key(6)]
        public int UnitsCaptured { get; }

        public ConflictEventArgs(HexPoint location, bool invasionSucceeded, Fleet invader, Fleet occupant, int unitsRetreated)
        {
            Location = location;
            InvasionSucceeded = invasionSucceeded;
            Invader = invader;
            Occupant = occupant;
            UnitsRetreated = unitsRetreated;
        }

        public ConflictEventArgs(HexPoint location, bool invasionSucceeded, Fleet invader, Fleet occupant, int unitsRetreated, int unitsDestroyed, int unitsCaptured) : this(location, invasionSucceeded, invader, occupant, unitsRetreated)
        {
            UnitsDestroyed = unitsDestroyed;
            UnitsCaptured = unitsCaptured;
        }
    }
}
