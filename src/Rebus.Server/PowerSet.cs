// Ishan Pranav's REBUS: PowerSet.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus.Server
{
    /// <summary>
    /// Provides a method to create a power set for a given set.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://stackoverflow.com/questions/19890781/creating-a-power-set-of-a-sequence">this</see> Stack Overflow question asked by <see href="https://stackoverflow.com/users/2625849/jamal-hussain">Jamal Hussain</see> and answered by <see href="https://stackoverflow.com/users/1740808/sergeys">@SergeyS</see>.
    /// </remarks>
    /// <seealso href="https://stackoverflow.com/questions/19890781/creating-a-power-set-of-a-sequence">c# - Creating a power set of a Sequence - Stack Overflow</seealso>
    /// <seealso href="https://stackoverflow.com/users/2625849/jamal-hussain">Jamal Hussain - Stack Overflow</seealso>
    /// <seealso href="https://stackoverflow.com/users/1740808/sergeys">@SergeyS - Stack Overflow</seealso>
    public static class PowerSet
    {
        /// <summary>
        /// Creates a power set for the given <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The set of values.</param>
        /// <returns>The set containing every subset of the given set, including the empty set (<c>{}</c>) and the given set (<paramref name="values"/>). The subsets are sorted in descending order based on their length.</returns>
        public static int[][] Create(IReadOnlyList<int> values)
        {
            int[][] result = new int[1 << values.Count][];

            result[0] = Array.Empty<int>();

            for (int i = 0; i < values.Count; i++)
            {
                int current = values[i];
                int count = 1 << i;

                for (int j = 0; j < count; j++)
                {
                    int[] source = result[j];
                    int[] destination = result[count + j] = new int[source.Length + 1];

                    for (int k = 0; k < source.Length; k++)
                    {
                        destination[k] = source[k];
                    }

                    destination[source.Length] = current;
                }
            }

            return result;
        }
    }
}
