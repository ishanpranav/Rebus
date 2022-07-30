// Ishan Pranav's REBUS: UnitViewModel.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using Rebus.Client.Windows.Attributes;

namespace Rebus.Client.Windows.Forms
{
    internal sealed class UnitViewModel
    {
        private readonly Func<HexPoint, string> _namer;

        [Browsable(false)]
        public Unit Value { get; }

        [LocalizedDisplayName(nameof(Selected))]
        public bool Selected { get; set; } = true;

        [LocalizedDisplayName(nameof(Sanctuary))]
        public string Sanctuary
        {
            get
            {
                if (Value.Sanctuary == null)
                {
                    return Resources.NoneMessage;
                }
                else
                {
                    return _namer(Value.Sanctuary.Location);
                }
            }
        }

        [LocalizedDisplayName(nameof(CargoMass))]
        public int? CargoMass
        {
            get
            {
                return Value.CargoMass;
            }
        }

        public UnitViewModel(Unit value, Func<HexPoint, string> namer)
        {
            Value = value;
            _namer = namer;
        }
    }
}
