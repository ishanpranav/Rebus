// Ishan Pranav's REBUS: RomanNumeralSystem.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Rebus.Server.NumeralSystems
{
    /// <summary>
    /// Represents the Roman numeral system.
    /// </summary>
    /// <remarks>
    /// The implementation of this class is an improvement of <see href="https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/RomanNumeralExtensions.cs">this</see> function attributed to <see href="https://github.com/jslicer">Jesse Slicer</see>. Humanizer is licensed under the MIT License.
    /// </remarks>
    /// <seealso href="https://github.com/Humanizr/Humanizer">Humanizer</seealso>
    /// <seealso href="https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/RomanNumeralExtensions.cs">Humanizer/RomanNumeralExtensions.cs</seealso>
    public class RomanNumeralSystem : INumeralSystem
    {
        /// <summary>
        /// Specifies the elementary Roman numerals using subtractive notation.
        /// </summary>
        /// <remarks>The original function relied on a specific ordering of key-value pairs in a <see cref="System.Collections.Generic.Dictionary{string,int}"/>. This is problematic because associative arrays are unsorted by definition. In this re-implementation, I have replaced the dictionary with two arrays: <see cref="s_numerals"/> and <see cref="s_integers"/>.</remarks>
        private static readonly string[] s_numerals = new string[]
        {
            "I",
            "IV",
            "V",
            "IX",
            "X",
            "XL",
            "L",
            "XC",
            "C",
            "CD",
            "D",
            "CM",
            "M",
        };

        /// <summary>
        /// Specifies the integer representations of the elements in <see cref="s_numerals"/>.
        /// </summary>
        /// <remarks>The original function relied on a specific ordering of key-value pairs in a <see cref="System.Collections.Generic.Dictionary{string,int}"/>. This is problematic because associative arrays are unsorted by definition. In this re-implementation, I have replaced the dictionary with two arrays: <see cref="s_numerals"/> and <see cref="s_integers"/>.</remarks>
        private static readonly int[] s_integers = new int[]
        {
            1,
            4,
            5,
            9,
            10,
            40,
            50,
            90,
            100,
            400,
            500,
            900,
            1000
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="RomanNumeralSystem"/> class.
        /// </summary>
        public RomanNumeralSystem() { }

        /// <inheritdoc/>
        public bool TryGetNumeral(int value, [NotNullWhen(true)] out string? result)
        {
            if (value < 4000)
            {
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = s_numerals.Length - 1; i >= 0; i--)
                {
                    int integer = s_integers[i];

                    while (value / integer > 0)
                    {
                        stringBuilder.Append(s_numerals[i]);

                        value -= integer;
                    }
                }

                result = stringBuilder.ToString();

                return true;
            }
            else
            {
                result = null;

                return false;
            }
        }
    }
}
