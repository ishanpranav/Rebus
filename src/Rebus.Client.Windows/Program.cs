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
                .AddSingleton(x =>
                {
                    Credentials result = x
                        .GetRequiredService<ObjectSaver>()
                        .Load<Credentials>();

                    result.ApplyCulture();

                    return result;
                })
                .AddSingleton<Lens, BiomeLens>()
                .AddSingleton<Lens, ConstellationLens>()
                .AddSingleton<Lens, PopulationLens>()
                .AddSingleton<Lens, StarLens>()
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
