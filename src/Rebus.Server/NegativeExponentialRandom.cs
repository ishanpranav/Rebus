// Ishan Pranav's REBUS: NegativeExponentialRandom.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace System
{
    /// <summary>
    /// Represents a pseudo-random number generator whose outputs, when aggregated, approximate a negative exponential distribution.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://stackoverflow.com/questions/20385964/generate-random-number-between-0-and-1-with-negativeexponential-distribution">this</see> Stack Overflow question asked by <see href="https://stackoverflow.com/users/1763110/maxammann">@maxammann</see> and answered by <see href="https://stackoverflow.com/users/2166798/pjs">@pjs</see>.
    /// </remarks>
    /// <seealso href="https://stackoverflow.com/questions/20385964/generate-random-number-between-0-and-1-with-negativeexponential-distribution">java - Generate random number between 0 and 1 with (negative)exponential distribution - Stack Overflow</seealso>
    /// <seealso href="https://stackoverflow.com/users/1763110/maxammann">@maxammann - Stack Overflow</seealso>
    /// <seealso href="https://stackoverflow.com/users/2166798/pjs">@pjs - Stack Overflow</seealso>
    public class NegativeExponentialRandom : Random
    {
        private readonly double _lambda;

        /// <inheritdoc cref="NegativeExponentialRandom(double)"/>
        /// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence. If a negative number is specified, the absolute value of the number is used.</param>
        /// <param name="lambda">The rate parameter <em>λ</em>.</param>
        public NegativeExponentialRandom(int seed, double lambda) : base(seed)
        {
            _lambda = lambda;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeExponentialRandom"/> class.
        /// </summary>
        /// <param name="lambda">The rate parameter <em>λ</em>.</param>
        public NegativeExponentialRandom(double lambda)
        {
            _lambda = lambda;
        }

        /// <inheritdoc/>
        protected override double Sample()
        {
            return -Math.Log(1 - (1 - Math.Exp(-_lambda)) * base.Sample()) / _lambda;
        }

        /// <inheritdoc/>
        public override double NextDouble()
        {
            return Sample();
        }

        /// <inheritdoc/>
        public override int Next(int minValue, int maxValue)
        {
            return (int)((NextDouble() * (maxValue - minValue)) + minValue);
        }
    }
}
