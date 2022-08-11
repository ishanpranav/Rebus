// Ishan Pranav's REBUS: Behavior.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Xml.Serialization;

namespace Rebus.Server.Considerations
{
    public class Behavior : Consideration
    {
        [XmlAttribute("command-type")]
        public CommandType CommandType { get; set; }

        [XmlElement("linear", typeof(LinearConsideration))]
        [XmlElement("quadratic", typeof(QuadraticConsideration))]
        public ConsiderationCollection Considerations { get; } = new ConsiderationCollection();

        public override double Evaluate(EvaluationContext context)
        {
            switch (Considerations.Count)
            {
                case 0:
                    return 0;

                case 1:
                    return Considerations[0].Evaluate(context);

                default:
                    return Math.Exp(Considerations.Sum(x => Math.Log(x.Evaluate(context))) / Considerations.Count);
            }
        }
    }
}
