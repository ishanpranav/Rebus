// Ishan Pranav's REBUS: DecisionMaker.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rebus.Server.Considerations
{
    [XmlRoot("behaviors")]
    public class BehaviorCollection : KeyedCollection<CommandType, Behavior>
    {
        public Arguments Select(SelectionContext context, IEnumerable<Arguments> decisions, out double maxEvaluation)
        {
            Arguments? maxDecision = default;

            maxEvaluation = 0;

            foreach (Arguments decision in decisions)
            {
                double evaluation = this[decision.Type].Evaluate(context);

                if (evaluation > maxEvaluation)
                {
                    maxDecision = decision;
                    maxEvaluation = evaluation;
                }
            }

            if (maxDecision == null)
            {
                return new Arguments(CommandType.None, Array.Empty<int>(), HexPoint.Empty);
            }
            else
            {
                return maxDecision;
            }
        }

        protected override CommandType GetKeyForItem(Behavior item)
        {
            return item.CommandType;
        }
    }
}
