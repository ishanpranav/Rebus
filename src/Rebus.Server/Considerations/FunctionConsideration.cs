// Ishan Pranav's REBUS: FunctionConsideration.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;
using System.Xml.Serialization;

namespace Rebus.Server.Considerations
{
    public abstract class FunctionConsideration : Consideration
    {
        [DefaultValue(1)]
        [XmlAttribute("a")]
        public double A { get; set; } = 1;

        [DefaultValue(1)]
        [XmlAttribute("b")]
        public double B { get; set; } = 1;

        [DefaultValue(0)]
        [XmlAttribute("h")]
        public double H { get; set; }

        [DefaultValue(0)]
        [XmlAttribute("k")]
        public double K { get; set; }

        [XmlText]
        public string X { get; set; } = string.Empty;

        public override double Evaluate(SelectionContext context)
        {
            return A * Evaluate(B * (context[X] - H)) + K;
        }

        protected abstract double Evaluate(double x);
    }
}
