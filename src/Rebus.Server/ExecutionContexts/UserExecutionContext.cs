// Ishan Pranav's REBUS: UserExecutionContext.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server.ExecutionContexts
{
    internal sealed class UserExecutionContext : ExecutionContext
    {
        private readonly User _user;

        public override Player Player
        {
            get
            {
                return _user.Player;
            }
        }

        public UserExecutionContext(User user, Controller controller, RebusDbContext dbContext, IReadOnlyCollection<int> unitIds, int commodity) : base(controller, dbContext, unitIds, commodity)
        {
            _user = user;
        }

        public override void SetLocation(HexPoint value)
        {
            _user.Location = value;

            base.SetLocation(value);
        }
    }
}
