// Ishan Pranav's REBUS: CommandType.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml.Serialization;

namespace Rebus
{
    public enum CommandType
    {
        [XmlEnum("none")]
        None = 0,

        [XmlEnum("autopilot")]
        Autopilot,

        [XmlEnum("defend")]
        Defend,

        [XmlEnum("explore")]
        Explore,

        [XmlEnum("jettison")]
        Jettison,

        [XmlEnum("purchase")]
        Purchase,

        [XmlEnum("retreat")]
        Retreat,

        [XmlEnum("sell")]
        Sell
    }
}
