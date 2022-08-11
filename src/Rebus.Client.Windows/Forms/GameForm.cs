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

        [NotNull]
        private Configuration? Configuration { get; set; }

        [NotNull]
        private GraphicsEngine? GraphicsEngine { get; set; }

        [NotNull]
        private IGameService? Service { get; set; }

        [NotNull]
        private Layout? GraphicsLayout { get; set; }

        [NotNull]
        private User? User { get; set; }

        public GameForm(IEnumerable<Lens> lenses, Credentials credentials)
        {
            InitializeComponent();

            _lenses = lenses;
            _credentials = credentials;
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMainFormLoad(object sender, EventArgs e)
        {
            myNotifyIcon.Icon = Resources.Icon;

            Service = await _client.CreateAsync<IGameService>(_credentials.IPAddress, _credentials.Port);

            Service.ConflictResolved += onConflictResolved;

            async void onConflictResolved(object? sender, ConflictEventArgs e)
            {
                if (e.Occupant.Username.Equals(User.Player.Name, StringComparison.OrdinalIgnoreCase))
                {
                    await Invoke(async () =>
                    {
                        User = await Service.GetUserAsync(_credentials.UserId);

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
                else if (e.Invader.Username.Equals(User.Player.Name, StringComparison.OrdinalIgnoreCase))
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

            Configuration = await Service.GetConfigurationAsync();

            const int hexagonSize = 16;
            int offset = ((Configuration.Radius * 2) + 1) * hexagonSize;

            GraphicsEngine = new GraphicsEngine(offset * 2, offset * 2);
            GraphicsLayout = new Layout(offset, offset)
            {
                HexagonWidth = hexagonSize,
                HexagonHeight = hexagonSize
            };

            User = await Service.GetUserAsync(_credentials.UserId);

            usernameToolStripLabel.Text = User.Player.Name;

            await RequestZonesAsync();

            foreach (Lens lens in _lenses)
            {
                lensComboBox.Items.Add(lens);
            }

            lensComboBox.SelectedIndex = 0;

            DrawZones();

            await RequestUnitsAsync();
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            myNotifyIcon.Dispose();
            Service.Dispose();

            await _client.DisposeAsync();

            Application.Exit();
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnVisionPictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            User.Location = GraphicsLayout.GetHexPoint(e.Location.X, e.Location.Y);

            myToolTip.ToolTipTitle = GetLocationName(User.Location);

            myToolTip.Show(User.Location.ToString(), visionPictureBox, e.Location);

            await RequestUnitsAsync();
        }

        private async Task DrawAsync()
        {
            myToolTip.ToolTipTitle = GetLocationName(User.Location);

            SKPoint point = GraphicsLayout.GetCenter(User.Location);

            myToolTip.Show(User.Location.ToString(), visionPictureBox, (int)point.X, (int)point.Y);

            await RequestZonesAsync();

            DrawZones();

            await RequestUnitsAsync();
        }

        private async Task RequestUnitsAsync()
        {
            unitListView.Items.Clear();
            descriptionTextBox.Clear();

            commandComboBox.DataSource = null;

            if (_zones.TryGetValue(User.Location, out ZoneInfo? zone) && zone.Units.Count > 0 && zone.PlayerId == User.Player.Id)
            {
                commandComboBox.DataSource = zone.Arguments
                    .Select(x => x.Type)
                    .Distinct()
                    .ToList();

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
                        GetCommodityName(unit.Commodity)
                    })
                    {
                        Checked = true,
                        Tag = unit.Id,
                        Text = unit.Name
                    });
                }

                if (commandComboBox.Items.Count > 0)
                {
                    commandComboBox.SelectedIndex = 0;
                }
            }

            economyListView.Items.Clear();

            Economy? economy = await Service.GetEconomyAsync(User.Player.Id, User.Location);

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
        }

        private async Task RequestZonesAsync()
        {
            _zones.Clear();

            await foreach (ZoneInfo zone in Service.GetZonesAsync(User.Player.Id))
            {
                _zones.Add(zone.Location, zone);
            }
        }

        private void DrawZones()
        {
            creditsToolStripLabel.Text = string.Format(Resources.CreditFormat, User.Player.Credits);

            visionPictureBox.Image?.Dispose();

            using (MemoryStream memoryStream = new MemoryStream())
            using (SKManagedWStream output = new SKManagedWStream(memoryStream))
            using (ZoneVisualizer drawable = new ZoneVisualizer(_zones.Values, (Lens)lensComboBox.SelectedItem, User.Player.Id, GraphicsLayout))
            {
                GraphicsEngine.Draw(output, SKEncodedImageFormat.Png, drawable);

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

        private void OnLensComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            DrawZones();
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnSubmitButtonClick(object sender, EventArgs e)
        {
            if (unitListView.CheckedItems.Count > 0 && commandComboBox.SelectedItem is CommandType type && destinationComboBox.SelectedItem is HexPoint destination)
            {
                Arguments arguments;
                HashSet<int> units = unitListView.CheckedItems
                    .Cast<ListViewItem>()
                    .Select(x => (int)x.Tag)
                    .ToHashSet();

                if (economyListView.CheckedItems.Count == 1)
                {
                    arguments = new Arguments(type, units, destination, (int)economyListView.CheckedItems[0].Tag);
                }
                else
                {
                    arguments = new Arguments(type, units, destination);
                }

                CommandResponse response = await Service.ExecuteAsync(new CommandRequest(_credentials.UserId, arguments));

                if (response.Modified)
                {
                    User = response.User;

                    await DrawAsync();
                }

                MessageBox.Show($"You know what you should've done? You should have sent units {string.Join(", ", response.Advice.Units)} to {s_resourceManager.GetString(response.Advice.Type.ToString())} at {GetLocationName(response.Advice.Destination)} (for commodity {GetCommodityName(response.Advice.Commodity)}). You really missed out.");
            }
        }

        private void RequestArguments()
        {
            int selectedIndex = destinationComboBox.SelectedIndex;

            destinationComboBox.DataSource = null;

            if (_zones.TryGetValue(User.Location, out ZoneInfo? source) && commandComboBox.SelectedItem is CommandType type)
            {
                destinationComboBox.DataSource = source.Arguments
                    .Where(x => x.Type == type)
                    .Select(x => x.Destination)
                    .Distinct()
                    .ToList();

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

        private void OnCommandComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            RequestArguments();
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
