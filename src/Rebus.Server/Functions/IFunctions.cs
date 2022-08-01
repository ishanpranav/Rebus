// Ishan Pranav's REBUS: IFunctions.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.Functions
{
    /// <summary>
    /// Defines graph-node functions used by search algorithms.
    /// </summary>
    /// <typeparam name="T">The type of each node in the graph.</typeparam>
    public interface IFunctions<T>
    {
        /// <summary>
        /// Performs the cost function.
        /// </summary>
        /// <param name="value">The source node.</param>
        /// <param name="neighbor">The neighbor.</param>
        /// <returns>The cost of traveling from the specified <paramref name="value"/> to its specified <paramref name="neighbor"/>.</returns>
        int Cost(T value, T neighbor);

        /// <summary>
        /// Performs the heuristic function.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="destination">The destination node.</param>
        /// <returns>The estimated distance from the <paramref name="source"/> to the <paramref name="destination"/>.</returns>
        int Heuristic(T source, T destination);

        /// <summary>
        /// Gets the neighbors of a node.
        /// </summary>
        /// <param name="value">The source node.</param>
        /// <returns>The neighbors of the specified <paramref name="value"/>.</returns>
        IEnumerable<T> Neighbors(T value);
    }
}
