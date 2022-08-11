// Ishan Pranav's REBUS: ArgumentProvider.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Rebus.Server.Commands;

namespace Rebus.Server.ArgumentProviders
{
    internal class ArgumentProvider
    {
        private readonly IReadOnlyDictionary<CommandType, ICommand> _commands;

        public ArgumentProvider(IReadOnlyDictionary<CommandType, ICommand> commands)
        {
            _commands = commands;
        }

        public IEnumerable<Arguments> GetArguments(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            if (source.Units.Count > 0)
            {
                foreach (ICommand command in _commands.Values)
                {
                    if (command is IDestinationProvider destinationProvider)
                    {
                        foreach (HexPoint destination in destinationProvider.GetDestinations(source, zones))
                        {
                            foreach (IReadOnlyList<int> unitIdCollection in GetUnitIdCollections(source))
                            {
                                yield return new Arguments(command.Type, unitIdCollection, destination);
                            }
                        }
                    }
                    else
                    {
                        foreach (IReadOnlyList<int> unitIdCollection in GetUnitIdCollections(source))
                        {
                            yield return new Arguments(command.Type, unitIdCollection, source.Location);
                        }
                    }
                }
            }
        }

        protected virtual IEnumerable<IReadOnlyList<int>> GetUnitIdCollections(ZoneInfo source)
        {
            yield return source.Units
                .Select(x => x.Id)
                .ToList();
        }
    }
}
