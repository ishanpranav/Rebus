// Ishan Pranav's REBUS: Command.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePack;

namespace Rebus.Commands
{
    [MessagePackObject]
    [Union(0, typeof(AutopilotCommand))]
    [Union(1, typeof(DefendCommand))]
    [Union(2, typeof(ExploreCommand))]
    [Union(3, typeof(JettisonCommand))]
    [Union(4, typeof(RetreatCommand))]
    public abstract class Command : ICloneable, IComparable, IComparable<Command>
    {
        [IgnoreMember]
        protected abstract int Priority { get; }

        [Key(0)]
        public ICollection<int> UnitIds { get; set; } = new HashSet<int>();

        [Key(1)]
        public int PlayerId { get; set; }

        [Key(2)]
        public HexPoint Destination { get; set; }

        public abstract Task<bool> ExecuteAsync(IExecutor executor);

        public virtual IEnumerable<HexPoint> Filter(ZoneResult source, IReadOnlyDictionary<HexPoint, ZoneResult> domain)
        {
            yield return source.Location;
        }

        public virtual Command Clone()
        {
            Command result = (Command)MemberwiseClone();

            result.UnitIds = new HashSet<int>();

            return result;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public int CompareTo(object? obj)
        {
            if (obj is Command other)
            {
                return CompareTo(other);
            }
            else
            {
                throw new ArgumentException("Argument is not a Command instance.", nameof(obj));
            }
        }

        public int CompareTo(Command? other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return Priority.CompareTo(other.Priority);
            }
        }
    }
}
