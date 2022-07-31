// Ishan Pranav's REBUS: Fleet.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.Serialization;

namespace Rebus
{
    [DataContract]
    public class Fleet
    {
        [DataMember(Order = 0)]
        public string Username { get; }

        [DataMember(Order = 1)]
        public int Size { get; }

        public Fleet(string username, int size)
        {
            Username = username;
            Size = size;
        }
    }
}
