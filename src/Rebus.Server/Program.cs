﻿// Ishan Pranav's REBUS: Program.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Server.Commands;
using Tracery;

namespace Rebus.Server
{
    internal static class Program
    {
        private static IServiceCollection AddObject<T>(this IServiceCollection source) where T : class
        {
            return source.AddSingleton(x => x
                .GetRequiredService<ObjectLoader>()
                .Load<T>());
        }

        private static void Main()
        {
            using (ServiceProvider serviceProvider = new ServiceCollection()
                .AddDbContextFactory<RebusDbContext>((serviceProvider, optionsBuilder) => optionsBuilder
                    .UseSqlite(serviceProvider
                        .GetRequiredService<IConfiguration>()
                        .GetConnectionString(nameof(RebusDbContext)))
                    .EnableSensitiveDataLogging())
                .AddLogging(x => x.AddConsole())
                .AddObject<Grammar>()
                .AddObject<Map>()
                .AddObject<RuleSet>()
                .AddObject<Table>()
                .AddSingleton<AStarSearch>()
                .AddSingleton<CantorPairing>()
                .AddSingleton<ICommand, AutopilotCommand>()
                .AddSingleton<ICommand, DefendCommand>()
                .AddSingleton<ICommand, ExploreCommand>()
                .AddSingleton<ICommand, JettisonCommand>()
                .AddSingleton<ICommand, NullCommand>()
                .AddSingleton<ICommand, PurchaseCommand>()
                .AddSingleton<ICommand, RetreatCommand>()
                .AddSingleton<ICommand, SellCommand>()
                .AddSingleton<Controller>()
                .AddSingleton<FisherYatesShuffle>()
                .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
                    .AddJsonFile(Path.ChangeExtension(path: "appSettings", extension: "json"))
                    .AddUserSecrets(typeof(Program).Assembly)
                    .Build())
                .AddSingleton<JsonConverter, JsonComplexConverter>()
                .AddSingleton<JsonConverter, JsonIPAddressConverter>()
                .AddSingleton(x =>
                {
                    StringCollectionLoader loader = x.GetRequiredService<StringCollectionLoader>();

                    return new Namer(x.GetRequiredService<FisherYatesShuffle>(), loader.Load(key: "Constellations"), loader.Load(key: "Stars"), loader.Load(key: "Planets"));
                })
                .AddSingleton(x => new Random(x.GetRequiredService<Table>().Seed))
                .AddSingleton(x =>
                {
                    Table table = x.GetRequiredService<Table>();

                    return new RpcListener(table.IPAddress, table.Port, x.GetRequiredService<ILogger<RpcListener>>(), x.GetRequiredService<Controller>());
                })
                .AddSingleton(x =>
                {
                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };

                    foreach (JsonConverter converter in x.GetServices<JsonConverter>())
                    {
                        options.Converters.Add(converter);
                    }

                    return new ObjectLoader(x
                        .GetRequiredService<IConfiguration>()
                        .GetConnectionString(nameof(ObjectLoader)), options);
                })
                .AddSingleton(x => new StringCollectionLoader(x
                    .GetRequiredService<IConfiguration>()
                    .GetConnectionString(nameof(StringCollectionLoader))))
                .BuildServiceProvider())
            {
                using (IServiceScope scope = serviceProvider.CreateScope())
                using (RebusDbContext context = scope.ServiceProvider.GetRequiredService<RebusDbContext>())
                {
                    context.Database.EnsureCreated();
                }

                serviceProvider
                   .GetRequiredService<RpcListener>()
                   .Start();
            }
        }
    }
}