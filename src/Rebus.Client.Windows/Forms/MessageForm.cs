// Ishan Pranav's REBUS: MessageForm.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rebus.Client.Windows.Forms
{
    internal sealed partial class MessageForm : FormBase
    {
        private readonly Credentials _credentials;
        private readonly RpcClient _client = new RpcClient();

#nullable disable
        private IMessageService _service;
#nullable enable

        public MessageForm(Credentials credentials)
        {
            InitializeComponent();

            _credentials = credentials;
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMessageFormLoad(object sender, System.EventArgs e)
        {
            _service = await _client.CreateAsync<IMessageService>(_credentials.IPAddress, _credentials.Port);

            _service.MessageReceived += (sender, e) => Invoke(() =>
            {
                outputRichTextBox.AppendText(e.Username);
                outputRichTextBox.AppendText(": ");
                outputRichTextBox.AppendText(e.Value);
                outputRichTextBox.AppendText("\n");
            });

            _client.Start();
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMessageFormClosing(object sender, FormClosingEventArgs e)
        {
            _service.Dispose();

            await _client.DisposeAsync();
        }

        private Task SendMessageAsync()
        {
            return _service.SendMessageAsync(_credentials.Username, inputRichTextBox.Text);
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnInputRichTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await SendMessageAsync();
            }
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnSendButtonClick(object sender, System.EventArgs e)
        {
            await SendMessageAsync();
        }
    }
}
