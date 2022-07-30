// Ishan Pranav's REBUS: WealthResult.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.Serialization;

namespace Rebus
{
    [DataContract]
    public class WealthResult
    {
        [DataMember(Order = 0)]
        public int Wealth { get; }

        [DataMember(Order = 1)]
        public int InterestPenalty { get; }

        public WealthResult(int wealth, int interestPenalty)
        {
            Wealth = wealth;
            InterestPenalty = interestPenalty;
        }
    }
}
