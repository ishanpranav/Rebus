// Ishan Pranav's REBUS: LoginForm.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Rebus.Client.Windows.Forms
{
    internal sealed partial class LoginForm : FormBase
    {
        private readonly IServiceProvider _serviceProvider;

        private Credentials _credentials;

        [MemberNotNull(nameof(_credentials))]
        public void SetCredentials(Credentials value)
        {
            _credentials = value;
            loginEntityGrid.SelectedObject = value;
            registerEntityGrid.SelectedObject = value;
        }

        public LoginForm(Credentials credentials, IServiceProvider serviceProvider, ObjectSaver saver)
        {
            InitializeComponent();
            SetCredentials(credentials);

            _serviceProvider = serviceProvider;
            loginEntityGrid.Saver = saver;
            registerEntityGrid.Saver = saver;
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnSubmitButtonClick(object sender, System.EventArgs e)
        {
            if (loginEntityGrid.ValidateObject())
            {
                _credentials.ApplyCulture();

                try
                {
                    await using (RpcClient client = new RpcClient())
                    {
                        using (ILoginService service = await client.CreateAsync<ILoginService>(_credentials.IPAddress, _credentials.Port))
                        {
                            client.Start();

                            bool isLogin = myTabControl.SelectedTab == loginTabPage;

                            if (isLogin)
                            {
                                _credentials.UserId = await service.LoginAsync(_credentials.Username, _credentials.Password);
                            }
                            else
                            {
                                _credentials.UserId = await service.RegisterAsync(_credentials.Username, _credentials.Password);
                            }

                            if (_credentials.UserId == default)
                            {
                                string errorMessage;

                                if (isLogin)
                                {
                                    errorMessage = Resources.LoginErrorMessage;
                                }
                                else
                                {
                                    errorMessage = Resources.RegisterErrorMessage;
                                }

                                MessageBox.Show(errorMessage, Resources.WarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                Hide();

                                _serviceProvider
                                    .GetRequiredService<GameForm>()
                                    .Show();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Resources.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void OnAboutButtonClick(object sender, System.EventArgs e)
        {
            using (AboutForm aboutForm = _serviceProvider.GetRequiredService<AboutForm>())
            {
                aboutForm.ShowDialog();
            }
        }
    }
}
