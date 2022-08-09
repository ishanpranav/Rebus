// Ishan Pranav's REBUS: AIExecutionContext.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.ExecutionContexts
{
    internal sealed class AIExecutionContext : ExecutionContext
    {
        private readonly User _user;

        public override Player Player
        {
            get
            {
                return _user.Twin;
            }
        }

        public AIExecutionContext(User user, Controller controller, RebusDbContext dbContext, IReadOnlyCollection<int> unitIds, int commodity) : base(controller, dbContext, unitIds, commodity)
        {
            _user = user;
        }
    }
}
