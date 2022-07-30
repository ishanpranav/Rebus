// Ishan Pranav's REBUS: ConflictEventArgs.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.Serialization;

namespace Rebus.EventArgs
{
    [DataContract]
    public class ConflictEventArgs : System.EventArgs
    {
        [DataMember(Order = 0)]
        public HexPoint Location { get; }

        [DataMember(Order = 1)]
        public bool InvasionSucceeded { get; }

        [DataMember(Order = 2)]
        public Fleet Invader { get; }

        [DataMember(Order = 3)]
        public Fleet Occupant { get; }

        [DataMember(Order = 4)]
        public int UnitsRetreated { get; }

        [DataMember(Order = 5)]
        public int UnitsDestroyed { get; }

        [DataMember(Order = 6)]
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
