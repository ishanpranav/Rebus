// Ishan Pranav's REBUS: GameForm.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Client.Lenses;
using Rebus.Commands;
using Rebus.EventArgs;
using SkiaSharp;

namespace Rebus.Client.Windows.Forms
{
    internal sealed partial class GameForm : FormBase
    {
        private static readonly ResourceManager s_resourceManager = new ResourceManager(baseName: "Rebus.Client.Windows.resources.Strings", typeof(GameForm).Assembly);

        private readonly IEnumerable<Command> _commands;
        private readonly IEnumerable<ILens> _lenses;
        private readonly Credentials _credentials;
        private readonly RpcClient _client = new RpcClient();
        private readonly Dictionary<HexPoint, ZoneResult> _zones = new Dictionary<HexPoint, ZoneResult>();

#nullable disable
        private Configuration _configuration;
        private GraphicsEngine _graphicsEngine;
        private IGameService _service;
        private Layout _layout;
#nullable enable

        private HexPoint _location;
        private int _credits;

        public GameForm(IEnumerable<Command> commands, IEnumerable<ILens> lenses, Credentials credentials)
        {
            InitializeComponent();

            _commands = commands;
            _lenses = lenses;
            _credentials = credentials;
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMainFormLoad(object sender, System.EventArgs e)
        {
            usernameToolStripLabel.Text = _credentials.Username;

            _service = await _client.CreateAsync<IGameService>(_credentials.IPAddress, _credentials.Port);

            _service.ConflictResolved += onConflictResolved;

            async void onConflictResolved(object? sender, ConflictEventArgs e)
            {
                if (e.Occupant.Username.Equals(_credentials.Username, StringComparison.OrdinalIgnoreCase))
                {
                    await Invoke(async () =>
                    {
                        await RequestAsync();

                        DrawAll();

                        if (e.InvasionSucceeded)
                        {
                            showBalloonTip(Resources.DefeatMessage, Resources.OccupantDefeatFormat);
                        }
                        else
                        {
                            showBalloonTip(Resources.VictoryMessage, Resources.OccupantVictoryFormat);
                        }
                    });
                }
                else if (e.Invader.Username.Equals(_credentials.Username, StringComparison.OrdinalIgnoreCase))
                {
                    if (e.InvasionSucceeded)
                    {
                        Invoke(() => showBalloonTip(Resources.VictoryMessage, Resources.InvaderVictoryFormat));
                    }
                    else
                    {
                        Invoke(() => showBalloonTip(Resources.DefeatMessage, Resources.InvaderDefeatFormat));
                    }
                }

                void showBalloonTip(string message, string format)
                {
                    myNotifyIcon.ShowBalloonTip(timeout: 1000, message, string.Format(format, e.Invader.Username, e.Location, e.Occupant.Size, e.Invader.Size, e.UnitsDestroyed, e.UnitsCaptured, e.UnitsRetreated, e.Occupant.Username), ToolTipIcon.Info);
                }
            };

            _client.Start();

            _configuration = await _service.ConfigureAsync();

            const int hexagonSize = 16;
            int offset = (_configuration.Radius * 2 + 1) * hexagonSize;

            _graphicsEngine = new GraphicsEngine(offset * 2, offset * 2);
            _layout = new Layout(offset, offset)
            {
                HexagonWidth = hexagonSize,
                HexagonHeight = hexagonSize
            };
            _credits = await _service.GetCreditsAsync(_credentials.PlayerId);

            DrawCredits();

            await RequestAsync();

            foreach (ILens lens in _lenses)
            {
                lensComboBox.Items.Add(lens);
            }

            lensComboBox.SelectedIndex = 0;

            commandComboBox.Items.Add(Resources.NoneMessage);

            foreach (Command command in _commands.OrderBy(x => x))
            {
                commandComboBox.Items.Add(command);
            }

            commandComboBox.SelectedIndex = 0;

            DrawZones();
            DrawUnits();

            myNotifyIcon.Icon = Resources.Icon;
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            myNotifyIcon.Dispose();
            _service.Dispose();

            await _client.DisposeAsync();

            Application.Exit();
        }

        private void OnVisionPictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            _location = _layout.GetHexPoint(e.Location.X, e.Location.Y);

            DrawUnits();

            if (_zones.TryGetValue(_location, out ZoneResult? zone))
            {
                myToolTip.ToolTipTitle = zone.Name;

                myToolTip.Show(_location.ToString(), visionPictureBox, e.Location);
            }
            else
            {
                myToolTip.ToolTipTitle = null;

                myToolTip.Show(_location.ToString(), visionPictureBox, e.Location);
            }
        }

        private void DrawAll()
        {
            DrawZones();
            DrawUnits();
            DrawDestinations();
        }

        private void DrawUnits()
        {
            unitListView.Items.Clear();

            if (_zones.TryGetValue(_location, out ZoneResult? zone) && zone.Units.Count > 0 && zone.PlayerId == _credentials.PlayerId)
            {
                foreach (Unit unit in zone.Units)
                {
                    string sanctuary;

                    if (unit.Sanctuary == null)
                    {
                        sanctuary = Resources.NoneMessage;
                    }
                    else
                    {
                        sanctuary = GetName(unit.Sanctuary.Location);
                    }

                    unitListView.Items.Add(new ListViewItem(new string[]
                    {
                        unit.Name,
                        sanctuary,
                        $"{unit.CargoMass}"
                    })
                    {
                        Checked = true,
                        Tag = unit.Id,
                        Text = unit.Name
                    });
                }

                commandComboBox.Enabled = true;
                destinationComboBox.Enabled = true;
                submitButton.Enabled = true;
            }
            else
            {
                commandComboBox.Enabled = false;
                destinationComboBox.Enabled = false;
                submitButton.Enabled = false;
            }
        }
        private async Task RequestAsync()
        {
            _zones.Clear();

            await foreach (ZoneResult zone in _service.GetZonesAsync(_credentials.PlayerId))
            {
                _zones.Add(zone.Location, zone);
            }
        }

        private void DrawZones()
        {
            visionPictureBox.Image?.Dispose();

            using (MemoryStream memoryStream = new MemoryStream())
            using (SKManagedWStream output = new SKManagedWStream(memoryStream))
            using (ZoneVisualizer drawable = new ZoneVisualizer(_zones.Values, (ILens)lensComboBox.SelectedItem, _credentials.PlayerId, _layout))
            {
                _graphicsEngine.Draw(output, SKEncodedImageFormat.Png, drawable);

                memoryStream.Position = 0;

                visionPictureBox.Image = Image.FromStream(memoryStream);
            }

            visionPictureBox.Refresh();
        }

        private void DrawCredits()
        {
            creditsToolStripLabel.Text = string.Format(Resources.CreditFormat, _credits);
        }

        private string GetName(HexPoint location)
        {
            if (_zones.TryGetValue(location, out ZoneResult? zone) && zone.Name != null)
            {
                return zone.Name;
            }
            else
            {
                return string.Format(Resources.ZoneFormat, location);
            }
        }

        private void OnLensComboBoxSelectedIndexChanged(object sender, System.EventArgs e)
        {
            DrawZones();
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnSubmitButtonClick(object sender, System.EventArgs e)
        {
            if (commandComboBox.SelectedItem is Command command && unitListView.CheckedItems.Count > 0)
            {
                Command clone = command.Clone();

                foreach (ListViewItem item in unitListView.CheckedItems)
                {
                    clone.UnitIds.Add((int)item.Tag);
                }

                clone.PlayerId = _credentials.PlayerId;
                clone.Destination = (HexPoint)destinationComboBox.SelectedItem;

                if (await _service.ExecuteAsync(clone))
                {
                    await RequestAsync();

                    DrawAll();
                }
            }
        }

        private void DrawDestinations()
        {
            if (commandComboBox.SelectedItem is Command command && _zones.TryGetValue(_location, out ZoneResult? source))
            {
                destinationComboBox.Items.Clear();

                foreach (HexPoint destination in command.Filter(source, _zones))
                {
                    destinationComboBox.Items.Add(destination);
                }

                destinationComboBox.SelectedIndex = 0;

                submitButton.Enabled = true;
            }
            else
            {
                submitButton.Enabled = false;
            }
        }

        private void OnCommandComboBoxSelectedIndexChanged(object sender, System.EventArgs e)
        {
            DrawDestinations();
        }

        private void OnDestinationComboBoxFormat(object sender, ListControlConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string) && e.ListItem is HexPoint destination)
            {
                e.Value = GetName(destination);
            }
        }

        private void OnFormat(object sender, ListControlConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string) && e.ListItem != null && e.ListItem is not string)
            {
                string key = e.ListItem.GetType().Name;

                e.Value = s_resourceManager.GetString(key) ?? key;
            }
        }
    }
}
