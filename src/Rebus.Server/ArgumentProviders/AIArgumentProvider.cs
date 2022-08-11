// Ishan Pranav's REBUS: AIArgumentProvider.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Rebus.Server.Commands;

namespace Rebus.Server.ArgumentProviders
{
    internal sealed class AIArgumentProvider : ArgumentProvider
    {
        public AIArgumentProvider(IReadOnlyDictionary<CommandType, ICommand> commands) : base(commands) { }

        protected override IEnumerable<IReadOnlyList<int>> GetUnitIdCollections(ZoneInfo source)
        {
            foreach (IReadOnlyList<int> unitIdCollection in base.GetUnitIdCollections(source))
            {
                int[][] powerSet = PowerSet.Create(unitIdCollection);

                for (int j = 1; j < powerSet.Length; j++)
                {
                    yield return powerSet[j];
                }
            }
        }
    }
}
