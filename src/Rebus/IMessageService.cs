// Ishan Pranav's REBUS: IMessageService.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Rebus.EventArgs;

namespace Rebus
{
    public interface IMessageService : IDisposable
    {
        event EventHandler<MessageEventArgs> MessageReceived;

        Task SendMessageAsync(string username, string value);
    }
}
