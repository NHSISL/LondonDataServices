// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Mesh;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Manual.OptOut
{
    public partial class OptOutTests
    {
        private readonly IOptOutClient optOutClient;
        private readonly IDocumentService documentService;
        private readonly IMeshService meshService;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly ITestOutputHelper output;

        public OptOutTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddOptOutClient(configuration)
                .BuildServiceProvider();

            this.optOutClient = serviceProvider.GetService<IOptOutClient>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.meshService = serviceProvider.GetService<IMeshService>();
            this.optOutConfiguration = serviceProvider.GetService<OptOutConfiguration>();
        }
    }
}