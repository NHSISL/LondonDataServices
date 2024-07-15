//// ---------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using LHDS.Core.Clients;
//using LHDS.Core.Clients.Extensions;
//using LHDS.Core.Models.Brokers.Storages.Blobs;
//using LHDS.Core.Models.Foundations.Mesh;
//using LHDS.Core.Models.Foundations.ResolvedAddresses;
//using LHDS.Core.Services.Foundations.Documents;
//using LHDS.Core.Services.Foundations.ResolvedAddresses;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Xunit.Abstractions;

//namespace LHDS.Core.Tests.Integration.Addresses
//{
//    public partial class AddressTests
//    {
//        private readonly IResolvedAddressService resolvedAddressService;
//        private readonly IAddressClient addressClient;
//        private readonly BlobContainers blobContainers;
//        private readonly IDocumentService documentService;
//        private readonly ITestOutputHelper output;

//        public AddressTests(ITestOutputHelper output)
//        {
//            this.output = output;
//            var environmentName = "Development";

//            var configurationBuilder = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
//                .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
//                .AddEnvironmentVariables();

//            IConfiguration configuration = configurationBuilder.Build();

//            //setup our DI
//            var serviceProvider = new ServiceCollection()
//                .AddLogging(builder =>
//                {
//                    builder.AddConsole();
//                    builder.AddApplicationInsights();
//                })
//                .AddAddressClient(configuration)
//                .BuildServiceProvider();

//            this.resolvedAddressService = serviceProvider.GetService<IResolvedAddressService>();
//            this.blobContainers = serviceProvider.GetService<BlobContainers>();
//            this.documentService = serviceProvider.GetService<IDocumentService>();
//            addressClient = serviceProvider.GetService<IAddressClient>();
//        }

//        private static string GetHeaderValue(MeshMessage message, string keyToFind)
//        {
//            List<string> value = new List<string>();

//            foreach (var key in message.Headers.Keys)
//            {
//                if (key.ToLower() == keyToFind.ToLower())
//                {
//                    message.Headers.TryGetValue(key, out value);

//                    break;
//                }
//            }

//            return value.FirstOrDefault();
//        }

//        private async ValueTask<List<ResolvedAddress>> SetupResolvedAddresses()
//        {
//            DateTimeOffset now = DateTimeOffset.UtcNow;
//            List<ResolvedAddress> resolvedAddresses = new List<ResolvedAddress>();

//            // First resolved address
//            ResolvedAddress resolvedAddress1 = new ResolvedAddress
//            {
//                Id = Guid.NewGuid(),
//                UniqueReference = Guid.NewGuid(),
//                BatchReference = Guid.NewGuid(),
//                UnstructuredPostalAddress = "",
//                PostCode = "",
//                PostalAddress = "",
//                JsonPostalAddress = "",
//                MatchAlgorithmEnum = MatchAlgorithmEnum.BestMatch,
//                IsMatched = true,
//                MatchedPostalAddress = "Test Matched Postal Address 1",
//                MatchedJsonPostalAddress = "Test Matched json 1",
//                MatchedUPRN = "Test Matched UPSN 1",
//                MatchedUPSN = "Test Matched UPRN 1",
//                MatchedOrganisationName = "Test Matched Org Name 1",
//                MatchedDepartmentName = "Test Matched Department Name 1",
//                MatchedSubBuildingName = "Test Matched Building Name 1",
//                MatchedBuildingName = "",
//                MatchedBuildingNumber = "",
//                MatchedDependentThoroughfare = "",
//                MatchedThoroughfare = "",
//                MatchedDoubleDependentLocality = "",
//                MatchedDependentLocality = "",
//                MatchedPostTown = "Test Matched Croydon 1",
//                MatchedPostCode = "CR2 0HG",
//                IsProcessed = false,
//                CreatedBy = "Test User",
//                UpdatedBy = "Test User",
//                UpdatedDate = now,
//                CreatedDate = now
//            };
//            resolvedAddresses.Add(resolvedAddress1);

//            // Second resolved address
//            ResolvedAddress resolvedAddress2 = new ResolvedAddress
//            {
//                Id = Guid.NewGuid(),
//                UniqueReference = Guid.NewGuid(),
//                BatchReference = Guid.NewGuid(),
//                UnstructuredPostalAddress = "",
//                PostCode = "",
//                PostalAddress = "",
//                JsonPostalAddress = "",
//                MatchAlgorithmEnum = MatchAlgorithmEnum.BestMatch,
//                IsMatched = true,
//                MatchedPostalAddress = "Test Matched Postal Address 2",
//                MatchedJsonPostalAddress = "Test Matched json 2",
//                MatchedUPRN = "Test Matched UPSN 2",
//                MatchedUPSN = "Test Matched UPRN 2",
//                MatchedOrganisationName = "Test Matched Org Name 2",
//                MatchedDepartmentName = "Test Matched Department Name 2",
//                MatchedSubBuildingName = "Test Matched Building Name 2",
//                MatchedBuildingName = "",
//                MatchedBuildingNumber = "",
//                MatchedDependentThoroughfare = "",
//                MatchedThoroughfare = "",
//                MatchedDoubleDependentLocality = "",
//                MatchedDependentLocality = "",
//                MatchedPostTown = "Test Matched Croydon 2",
//                MatchedPostCode = "CR2 0HG",
//                IsProcessed = false,
//                CreatedBy = "Test User",
//                UpdatedBy = "Test User",
//                UpdatedDate = now,
//                CreatedDate = now
//            };
//            resolvedAddresses.Add(resolvedAddress2);

//            // Third resolved address
//            ResolvedAddress resolvedAddress3 = new ResolvedAddress
//            {
//                Id = Guid.NewGuid(),
//                UniqueReference = Guid.NewGuid(),
//                BatchReference = Guid.NewGuid(),
//                UnstructuredPostalAddress = "",
//                PostCode = "",
//                PostalAddress = "",
//                JsonPostalAddress = "",
//                MatchAlgorithmEnum = MatchAlgorithmEnum.BestMatch,
//                IsMatched = true,
//                MatchedPostalAddress = "Test Matched Postal Address 3",
//                MatchedJsonPostalAddress = "Test Matched json 3",
//                MatchedUPRN = "Test Matched UPSN 3",
//                MatchedUPSN = "Test Matched UPRN 3",
//                MatchedOrganisationName = "Test Matched Org Name 3",
//                MatchedDepartmentName = "Test Matched Department Name 3",
//                MatchedSubBuildingName = "Test Matched Building Name 3",
//                MatchedBuildingName = "",
//                MatchedBuildingNumber = "",
//                MatchedDependentThoroughfare = "",
//                MatchedThoroughfare = "",
//                MatchedDoubleDependentLocality = "",
//                MatchedDependentLocality = "",
//                MatchedPostTown = "Test Matched Croydon 3",
//                MatchedPostCode = "CR2 0HG",
//                IsProcessed = false,
//                CreatedBy = "Test User",
//                UpdatedBy = "Test User",
//                UpdatedDate = now,
//                CreatedDate = now
//            };
//            resolvedAddresses.Add(resolvedAddress3);

//            // Add all resolved addresses to the database
//            List<ResolvedAddress> addedResolvedAddresses = new List<ResolvedAddress>();
//            foreach (var resolvedAddress in resolvedAddresses)
//            {
//                var maybeResolvedAddress = resolvedAddressService.RetrieveAllResolvedAddresses()
//                    .FirstOrDefault(s => s.Id == resolvedAddress.Id);

//                if (maybeResolvedAddress == null)
//                {
//                    addedResolvedAddresses.Add(await resolvedAddressService.AddResolvedAddressAsync(resolvedAddress));
//                }
//                else
//                {
//                    addedResolvedAddresses.Add(maybeResolvedAddress);
//                }
//            }

//            return addedResolvedAddresses;
//        }

//    }
//}
