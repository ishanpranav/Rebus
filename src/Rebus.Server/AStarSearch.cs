// Ishan Pranav's REBUS: AStarSearch.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Rebus.Server.Functions;

namespace Rebus.Server
{
    /// <summary>
    /// Provides an implementation of the A* search algorithm. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/A*_search_algorithm">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/A*_search_algorithm">A* search algorithm - Wikipedia</seealso>
    public static class AStarSearch
    {
        /// <summary>
        /// Finds the shortest path between two nodes.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="destination">The destination node.</param>
        /// <param name="functions">The set of graph-node functions.</param>
        /// <returns>A stack containing the ordered set of nodes leading from the <paramref name="source"/> node to the <paramref name="destination"/> node, or an empty stack if no such path exists.</returns>
        public static Stack<T> Search<T>(T source, T destination, IFunctions<T> functions) where T : IEquatable<T>
        {
            // function A_Star(start, goal, h)

            // openSet := {start}
            // ...
            // fScore := map with default value of Infinity
            // ...

            PriorityQueue<T, int> openSet = new();

            // ...
            // fScore[start] := h(start)
            // ...

            openSet.Enqueue(source, functions.Heuristic(source, destination));

            // cameFrom := an empty map

            Dictionary<T, T> previousNodes = new();

            // gScore := map with default value of Infinity
            // gScore[start] := 0

            Dictionary<T, int> cumulativeCosts = new()
            {
                { source, 0 }
            };

            // while openSet is not empty
            //   current := the node in openSet having the lowest fScore[] value
            // ...
            //   openSet.Remove(current)
            // ...

            while (openSet.TryDequeue(out T? current, out _))
            {
                // if current = goal

                if (current.Equals(destination))
                {
                    // return reconstruct_path(cameFrom, current)

                    // function reconstruct_path(cameFrom, current)
                    //   total_path := {current}

                    Stack<T> results = new(previousNodes.Count);

                    results.Push(current);

                    // while current in cameFrom.Keys
                    //   current := cameFrom[current]

                    while (previousNodes.TryGetValue(current, out current))
                    {
                        // total_path.prepend(current)

                        results.Push(current);
                    }

                    return results;
                }
                else
                {
                    // for each neighbor of current

                    foreach (T neighbor in functions.Neighbors(current))
                    {
                        // tentative_gScore := gScore[current] + d(current, neighbor)

                        int tenativeCumulativeCost = cumulativeCosts[current] + functions.Cost(current, neighbor);

                        // if tentative_gScore < gScore[neighbor]

                        if (!cumulativeCosts.ContainsKey(neighbor) || tenativeCumulativeCost < cumulativeCosts[neighbor])
                        {
                            // cameFrom[neighbor] := current

                            previousNodes[neighbor] = current;

                            // gScore[neighbor] := tentative_gScore

                            cumulativeCosts[neighbor] = tenativeCumulativeCost;

                            // fScore[neighbor] := tentative_gScore + h(neighbor)
                            // if neighbor not in openSet
                            //   openSet.add(neighbor)

                            openSet.Enqueue(neighbor, tenativeCumulativeCost + functions.Heuristic(neighbor, destination));
                        }
                    }
                }
            }

            // return failure

            return new Stack<T>();
        }
    }
}
