// Ishan Pranav's REBUS: CommandProvider.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Rebus.Server.Commands;

namespace Rebus.Server.CommandProviders
{
    internal class CommandProvider
    {
        private readonly IReadOnlyDictionary<CommandType, ICommand> _commands;

        public CommandProvider(IReadOnlyDictionary<CommandType, ICommand> commands)
        {
            _commands = commands;
        }

        public IReadOnlyCollection<Arguments> GetCommands(ZoneInfo source, IReadOnlyDictionary<HexPoint, ZoneInfo> zones)
        {
            HashSet<Arguments> results = new HashSet<Arguments>();

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
                                results.Add(new Arguments(command.Type, unitIdCollection, destination));
                            }
                        }
                    }
                    else
                    {
                        foreach (IReadOnlyList<int> unitIdCollection in GetUnitIdCollections(source))
                        {
                            results.Add(new Arguments(command.Type, unitIdCollection, source.Location));
                        }
                    }
                }
            }

            return results;
        }

        protected virtual IEnumerable<IReadOnlyList<int>> GetUnitIdCollections(ZoneInfo source)
        {
            yield return source.Units
                .Select(x => x.Id)
                .ToList();
        }
    }
}
