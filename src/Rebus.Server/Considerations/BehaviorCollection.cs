// Ishan Pranav's REBUS: BehaviorCollection.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.ObjectModel;

namespace Rebus.Server.Considerations
{
    public class BehaviorCollection : KeyedCollection<CommandType, Behavior>
    {
        protected override CommandType GetKeyForItem(Behavior item)
        {
            return item.CommandType;
        }
    }
}
