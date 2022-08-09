// Ishan Pranav's REBUS: ILoginService.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Rebus
{
    public interface ILoginService : IDisposable
    {
        Task<int> LoginAsync(string name, string password);
        Task<int> RegisterAsync(string name, string password);
    }
}
