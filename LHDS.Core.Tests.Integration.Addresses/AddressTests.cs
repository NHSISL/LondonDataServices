// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        private readonly IResolvedAddressService resolvedAddressService;
        private readonly IAddressClient addressClient;
        private readonly BlobContainers blobContainers;
        private readonly IDocumentService documentService;
        private readonly ITestOutputHelper output;

        public AddressTests(ITestOutputHelper output)
        {
            this.output = output;
            var environmentName = "Development";

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "ISL.LDS.AdminSpa.Administrators")
            }, "TestAuthType"));

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddAddressClient(configuration, claimsPrincipal)
                .BuildServiceProvider();

            this.resolvedAddressService = serviceProvider.GetService<IResolvedAddressService>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            addressClient = serviceProvider.GetService<IAddressClient>();
        }

        private static string GetHeaderValue(MeshMessage message, string keyToFind)
        {
            List<string> value = new List<string>();

            foreach (var key in message.Headers.Keys)
            {
                if (key.ToLower() == keyToFind.ToLower())
                {
                    message.Headers.TryGetValue(key, out value);

                    break;
                }
            }

            return value.FirstOrDefault();
        }
    }
}
