// Ishan Pranav's REBUS: Agent.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rebus.Server.Considerations
{
    [XmlRoot("agent")]
    public class Agent
    {
        [XmlElement("behavior")]
        public BehaviorCollection Behaviors { get; } = new BehaviorCollection();

        public Arguments Evaluate(EvaluationContext context, IReadOnlyList<Arguments> decisions)
        {
            if (decisions.Count == 0)
            {
                return new Arguments(CommandType.None, Array.Empty<int>(), HexPoint.Empty);
            }
            else
            {
                Arguments result = decisions[0];

                for (int i = 1; i < decisions.Count; i++)
                {
                    Arguments decision = decisions[i];

                    result.Evaluation = Behaviors[decision.Type].Evaluate(context);

                    if (decision.Evaluation > result.Evaluation)
                    {
                        result = decision;
                    }
                }

                return result;
            }
        }
    }
}
