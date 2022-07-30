// Ishan Pranav's REBUS: 
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public interface IExecutor
    {
        Task<bool> RetreatAsync(ICollection<int> unitIds, int playerId, HexPoint destination);
        Task<bool> DefendAsync(ICollection<int> unitIds);
        Task<bool> JettisonAsync(ICollection<int> unitIds);
        Task<bool> MoveAsync(ICollection<int> unitIds, int playerId, HexPoint destination);
    }
}
