// Ishan Pranav's REBUS: Table.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Net;

namespace Rebus.Server
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
