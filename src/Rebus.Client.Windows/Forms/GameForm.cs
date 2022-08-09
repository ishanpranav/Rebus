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
using Rebus.Client.Lenses;
using Rebus.EventArgs;
using SkiaSharp;

namespace Rebus.Client.Windows.Forms
{
    internal sealed partial class GameForm : FormBase
    {
        private static readonly ResourceManager s_resourceManager = new(baseName: "Rebus.Client.Windows.resources.Strings", typeof(GameForm).Assembly);

        private readonly IEnumerable<Lens> _lenses;
        private readonly Credentials _credentials;
        private readonly RpcClient _client = new RpcClient();
        private readonly Dictionary<HexPoint, ZoneInfo> _zones = new Dictionary<HexPoint, ZoneInfo>();

#nullable disable
        private Configuration _configuration;
        private GraphicsEngine _graphicsEngine;
        private IGameService _service;
        private Layout _layout;
        private User _user;
#nullable enable

        public GameForm(IEnumerable<Lens> lenses, Credentials credentials)
        {
            InitializeComponent();

            _lenses = lenses;
            _credentials = credentials;
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMainFormLoad(object sender, System.EventArgs e)
        {
            myNotifyIcon.Icon = Resources.Icon;

            _service = await _client.CreateAsync<IGameService>(_credentials.IPAddress, _credentials.Port);

            _service.ConflictResolved += onConflictResolved;

            async void onConflictResolved(object? sender, ConflictEventArgs e)
            {
                if (e.Occupant.Username.Equals(_user.Player.Name, StringComparison.OrdinalIgnoreCase))
                {
                    await Invoke(async () =>
                    {
                        _user = await _service.GetUserAsync(_credentials.UserId);

                        await DrawAsync();

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
                else if (e.Invader.Username.Equals(_user.Player.Name, StringComparison.OrdinalIgnoreCase))
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

            _configuration = await _service.GetConfigurationAsync();

            const int hexagonSize = 16;
            int offset = ((_configuration.Radius * 2) + 1) * hexagonSize;

            _graphicsEngine = new GraphicsEngine(offset * 2, offset * 2);
            _layout = new Layout(offset, offset)
            {
                HexagonWidth = hexagonSize,
                HexagonHeight = hexagonSize
            };

            _user = await _service.GetUserAsync(_credentials.UserId);

            usernameToolStripLabel.Text = _user.Player.Name;

            await RequestZonesAsync();

            foreach (Lens lens in _lenses)
            {
                lensComboBox.Items.Add(lens);
            }

            lensComboBox.SelectedIndex = 0;

            commandComboBox.DataSource = Enum.GetValues<CommandType>();

            DrawZones();

            await RequestUnitsAsync();
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            myNotifyIcon.Dispose();
            _service.Dispose();

            await _client.DisposeAsync();

            Application.Exit();
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnVisionPictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            _user.Location = _layout.GetHexPoint(e.Location.X, e.Location.Y);

            await RequestUnitsAsync();

            myToolTip.ToolTipTitle = GetLocationName(_user.Location);

            myToolTip.Show(_user.Location.ToString(), visionPictureBox, e.Location);
        }

        private async Task DrawAsync()
        {
            await RequestZonesAsync();

            DrawZones();

            await RequestUnitsAsync();
        }

        private async Task RequestUnitsAsync()
        {
            unitListView.Items.Clear();
            descriptionTextBox.Clear();

            if (_zones.TryGetValue(_user.Location, out ZoneInfo? zone) && zone.Units.Count > 0 && zone.PlayerId == _user.Player.Id)
            {
                foreach (Unit unit in zone.Units)
                {
                    string sanctuary;

                    if (unit.SanctuaryLocation.HasValue)
                    {
                        sanctuary = GetLocationName(unit.SanctuaryLocation.Value);
                    }
                    else
                    {
                        sanctuary = Resources.NoneMessage;
                    }

                    unitListView.Items.Add(new ListViewItem(new string[]
                    {
                        unit.Name,
                        sanctuary,
                        GetCommodityName(unit.CargoMass)
                    })
                    {
                        Checked = true,
                        Tag = unit.Id,
                        Text = unit.Name
                    });
                }
            }
            else
            {
                commandComboBox.SelectedItem = CommandType.Autopilot;

                if (destinationComboBox.Items.Contains(_user.Location))
                {
                    destinationComboBox.SelectedItem = _user.Location;
                }
            }

            economyListView.Items.Clear();

            Economy? economy = await _service.GetEconomyAsync(_user.Player.Id, _user.Location);

            if (economy != null)
            {
                foreach (Commodity commodity in economy.Commodities)
                {
                    if (commodity.Quantity < 0)
                    {
                        add(group: 0, -commodity.Quantity);
                    }
                    else
                    {
                        add(group: 1, commodity.Quantity);
                    }

                    void add(int group, int quantity)
                    {
                        string name = GetCommodityName(commodity.Mass);

                        economyListView.Items.Add(new ListViewItem(new string[]
                        {
                            name,
                            string.Format(Resources.PriceFormat, commodity.Price),
                            string.Format(Resources.QuantityFormat, quantity)
                        })
                        {
                            Group = economyListView.Groups[group],
                            Text = name,
                            Tag = commodity.Mass
                        });
                    }
                }

                descriptionTextBox.Text = economy.Description;
            }

            await RequestDestinationsAsync();
        }

        private async Task RequestZonesAsync()
        {
            _zones.Clear();

            await foreach (ZoneInfo zone in _service.GetZonesAsync(_user.Player.Id))
            {
                _zones.Add(zone.Location, zone);
            }
        }

        private void DrawZones()
        {
            creditsToolStripLabel.Text = string.Format(Resources.CreditFormat, _user.Player.Credits);

            visionPictureBox.Image?.Dispose();

            using (MemoryStream memoryStream = new MemoryStream())
            using (SKManagedWStream output = new SKManagedWStream(memoryStream))
            using (ZoneVisualizer drawable = new ZoneVisualizer(_zones.Values, (Lens)lensComboBox.SelectedItem, _user.Player.Id, _layout))
            {
                _graphicsEngine.Draw(output, SKEncodedImageFormat.Png, drawable);

                memoryStream.Position = 0;

                visionPictureBox.Image = Image.FromStream(memoryStream);
            }

            visionPictureBox.Refresh();
        }

        private static string GetCommodityName(int mass)
        {
            if (mass == 0)
            {
                return Resources.NoneMessage;
            }
            else
            {
                return s_resourceManager.GetString($"Element{mass}") ?? mass.ToString();
            }
        }

        private string GetLocationName(HexPoint location)
        {
            if (_zones.TryGetValue(location, out ZoneInfo? zone) && zone.Name != null)
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
            if (unitListView.CheckedItems.Count > 0 && commandComboBox.SelectedItem is CommandType type && destinationComboBox.SelectedItem is HexPoint destination)
            {
                HashSet<int> unitIds = unitListView.CheckedItems
                    .Cast<ListViewItem>()
                    .Select(x => (int)x.Tag)
                    .ToHashSet();
                CommandRequest request;

                if (economyListView.CheckedItems.Count == 1)
                {
                    request = new CommandRequest(type, _credentials.UserId, unitIds, destination, (int)economyListView.CheckedItems[0].Tag);
                }
                else
                {
                    request = new CommandRequest(type, _credentials.UserId, unitIds, destination);
                }

                CommandResponse response = await _service.ExecuteAsync(request);

                if (response.Modified)
                {
                    _user = response.User;

                    await DrawAsync();
                }
            }
        }

        private async Task RequestDestinationsAsync()
        {
            int selectedIndex = destinationComboBox.SelectedIndex;

            destinationComboBox.Items.Clear();

            if (_zones.TryGetValue(_user.Location, out ZoneInfo? source) && commandComboBox.SelectedItem is CommandType type)
            {
                await foreach (HexPoint destination in _service.GetDestinationsAsync(_user.Player.Id, type, source))
                {
                    destinationComboBox.Items.Add(destination);
                }

                if (selectedIndex != -1 && destinationComboBox.Items.Count > selectedIndex)
                {
                    destinationComboBox.SelectedIndex = selectedIndex;
                }
                else if (destinationComboBox.Items.Count > 0)
                {
                    destinationComboBox.SelectedIndex = 0;
                }
            }
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnCommandComboBoxSelectedIndexChanged(object sender, System.EventArgs e)
        {
            await RequestDestinationsAsync();
        }

        private void OnDestinationComboBoxFormat(object sender, ListControlConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string) && e.ListItem is HexPoint destination)
            {
                e.Value = GetLocationName(destination);
            }
        }

        private void OnLensComboBoxFormat(object sender, ListControlConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string) && e.ListItem != null && e.ListItem is not string)
            {
                string key = e.ListItem.GetType().Name;

                e.Value = s_resourceManager.GetString(key) ?? key;
            }
        }

        private void OnCommandComboBoxFormat(object sender, ListControlConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string) && e.ListItem != null)
            {
                string key = ((CommandType)e.ListItem).ToString();

                e.Value = s_resourceManager.GetString(key) ?? key;
            }
        }

        private void OnEconomyListViewItemCheck(object sender, ItemCheckEventArgs e)
        {
            int count = economyListView.CheckedItems.Count;

            if (e.NewValue == CheckState.Checked)
            {
                count++;
            }
            else
            {
                count--;
            }

            if (count > 1)
            {
                economyListView.CheckedItems[0].Checked = false;

                count--;
            }

            if (count == 1)
            {
                if (string.Equals(economyListView.Items[e.Index].Group.Name, economyListView.Groups[0].Name))
                {
                    commandComboBox.SelectedItem = CommandType.Sell;
                }
                else
                {
                    commandComboBox.SelectedItem = CommandType.Purchase;
                }
            }
        }
    }
}
