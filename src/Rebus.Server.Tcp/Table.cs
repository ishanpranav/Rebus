// Ishan Pranav's REBUS: Table.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Rebus.Server.Tcp
{
    internal sealed class Table
    {
        public int Seed { get; }
        public int Port { get; }
        public IPAddress IPAddress { get; }

        public Table(int seed, int port, IPAddress iPAddress)
        {
            Seed = seed;
            Port = port;
            IPAddress = iPAddress;
        }
    }
}
