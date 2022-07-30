// Ishan Pranav's REBUS: MessageEventArgs.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Rebus.EventArgs
{
    [DataContract]
    public class MessageEventArgs : System.EventArgs
    {
        [DataMember(Order = 0)]
        public string Username { get; }

        [DataMember(Order = 1)]
        public string Value { get; }

        public MessageEventArgs(string username, string value)
        {
            Username = username;
            Value = value;
        }
    }
}
