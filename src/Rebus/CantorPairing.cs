// Ishan Pranav's REBUS: CantorPairing.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Provides an implementation of the Cantor pairing function.
    /// </summary>
    /// <remarks>The function in this class comes from <see href="https://en.wikipedia.org/wiki/Pairing_function#Cantor_pairing_function">this</see> Wikipedia article.</remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Pairing_function#Cantor_pairing_function">Pairing function - Wikipedia</seealso>
    public class CantorPairing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CantorPairing"/> class.
        /// </summary>
        public CantorPairing() { }

        /// <summary>
        /// Assigns a natural number to the pair of natural numbers.
        /// </summary>
        /// <param name="value1">The first natural number.</param>
        /// <param name="value2">The second natural number.</param>
        /// <returns>A natural number unique to the pair of <paramref name="value1"/> and <paramref name="value2"/>.</returns>
        public int Pair(int value1, int value2)
        {
            int sum = value1 + value2;

            return (sum * (sum + 1) / 2) + value2;
        }
    }
}
