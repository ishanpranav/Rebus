// Ishan Pranav's REBUS: ICommand.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Rebus.Server.ExecutionContexts;

namespace Rebus.Server.Commands
{
    internal interface ICommand
    {
        CommandType Type { get; }

        Task ExecuteAsync(ExecutionContext context);
    }
}
