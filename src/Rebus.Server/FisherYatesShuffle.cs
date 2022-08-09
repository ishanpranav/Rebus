// Ishan Pranav's REBUS: FisherYatesShuffle.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Server
{
    /// <summary>
    /// Performs Durstenfeld's Fisher–Yates shuffle algorithm to randomize collections.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher–Yates shuffle - Wikipedia</seealso>
    public class FisherYatesShuffle
    {
        /// <summary>
        /// Gets the pseudo-random number generator.
        /// </summary>
        /// <value>The pseudo-random number generator.</value>
        public Random Random { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FisherYatesShuffle"/> class.
        /// </summary>
        /// <param name="random">The pseudo-random number generator.</param>
        public FisherYatesShuffle(Random random)
        {
            Random = random;
        }

        /// <summary>
        /// Shuffles a collection using Durstenfeld's algorithm.
        /// </summary>
        /// <param name="values">The collection.</param>
        public void Shuffle(IList<string> values)
        {
            // for i from n−1 downto 1 do

            for (int i = values.Count - 1; i >= 1; i--)
            {
                // j ← random integer such that 0 ≤ j ≤ i

                int j = Random.Next(i + 1);

                // exchange a[j] and a[i]

                (values[i], values[j]) = (values[j], values[i]);
            }
        }
    }
}
