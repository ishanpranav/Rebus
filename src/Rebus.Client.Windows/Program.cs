// Ishan Pranav's REBUS: Program.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Client.Lenses;
using Rebus.Client.Windows.Forms;
using Rebus.Commands;

namespace Rebus.Client.Windows
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<AboutForm>()
                .AddTransient<GameForm>()
                .AddTransient<LoginForm>()
                .AddTransient<MessageForm>()
                .AddSingleton<Command, AutopilotCommand>()
                .AddSingleton<Command, DefendCommand>()
                .AddSingleton<Command, ExploreCommand>()
                .AddSingleton<Command, JettisonCommand>()
                .AddSingleton<Command, RetreatCommand>()
                .AddSingleton(x =>
                {
                    Credentials result = x
                        .GetRequiredService<ObjectSaver>()
                        .Load<Credentials>();

                    result.ApplyCulture();

                    return result;
                })
                .AddSingleton<ILens, BiomeLens>()
                .AddSingleton<ILens, ConstellationLens>()
                .AddSingleton<JsonConverter, JsonCultureInfoConverter>()
                .AddSingleton<JsonConverter, JsonIPAddressConverter>()
                .AddSingleton(x =>
                {
                    JsonSerializerOptions result = new JsonSerializerOptions()
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };

                    foreach (JsonConverter converter in x.GetServices<JsonConverter>())
                    {
                        result.Converters.Add(converter);
                    }

                    return result;
                })
                .AddSingleton<NoticeParser>()
                .AddSingleton<ObjectSaver>()
                .BuildServiceProvider();

            Application.ApplicationExit += onApplicationExit;

            async void onApplicationExit(object? sender, System.EventArgs e)
            {
                await serviceProvider.DisposeAsync();
            }

            Application.Run(serviceProvider.GetRequiredService<LoginForm>());
        }
    }
}
