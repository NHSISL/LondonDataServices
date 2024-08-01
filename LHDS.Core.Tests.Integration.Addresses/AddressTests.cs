// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        private readonly IAddressOrchestrationService addressOrchestrationService;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;
        private readonly IResolvedAddressProcessingService resolvedAddressProcessingService;
        private readonly IDocumentService documentService;
        private readonly IAddressService addressService;
        private readonly IResolvedAddressService resolvedAddressService;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IAddressClient addressClient;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly AddressConfiguration addressConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly WireMockServer wireMockServer;
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

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddApplicationInsights();
                })
                .AddAddressClient(configuration)
                .BuildServiceProvider();

            this.resolvedAddressService = serviceProvider.GetService<IResolvedAddressService>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
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

        private static string GetRandomString() =>
           new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private async ValueTask<List<ResolvedAddress>> SetupResolvedAddresses()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            List<ResolvedAddress> resolvedAddresses = new List<ResolvedAddress>();

            // First resolved address
            ResolvedAddress resolvedAddress1 = new ResolvedAddress
            {
                Id = Guid.NewGuid(),
                UniqueReference = Guid.NewGuid(),
                BatchReference = Guid.NewGuid(),
                UnstructuredPostalAddress = "",
                PostCode = "",
                CreatedBy = "Test User",
                UpdatedBy = "Test User",
                UpdatedDate = now,
                CreatedDate = now
            };
            resolvedAddresses.Add(resolvedAddress1);

            // Second resolved address
            ResolvedAddress resolvedAddress2 = new ResolvedAddress
            {
                Id = Guid.NewGuid(),
                UniqueReference = Guid.NewGuid(),
                BatchReference = Guid.NewGuid(),
                UnstructuredPostalAddress = "",
                PostCode = "",
                CreatedBy = "Test User",
                UpdatedBy = "Test User",
                UpdatedDate = now,
                CreatedDate = now
            };
            resolvedAddresses.Add(resolvedAddress2);

            // Third resolved address
            ResolvedAddress resolvedAddress3 = new ResolvedAddress
            {
                Id = Guid.NewGuid(),
                UniqueReference = Guid.NewGuid(),
                BatchReference = Guid.NewGuid(),
                UnstructuredPostalAddress = "",
                PostCode = "",
                CreatedBy = "Test User",
                UpdatedBy = "Test User",
                UpdatedDate = now,
                CreatedDate = now
            };
            resolvedAddresses.Add(resolvedAddress3);

            // Add all resolved addresses to the database
            List<ResolvedAddress> addedResolvedAddresses = new List<ResolvedAddress>();
            foreach (var resolvedAddress in resolvedAddresses)
            {
                var maybeResolvedAddress = resolvedAddressService.RetrieveAllResolvedAddresses()
                    .FirstOrDefault(s => s.Id == resolvedAddress.Id);

                if (maybeResolvedAddress == null)
                {
                    addedResolvedAddresses.Add(await resolvedAddressService.AddResolvedAddressAsync(resolvedAddress));
                }
                else
                {
                    addedResolvedAddresses.Add(maybeResolvedAddress);
                }
            }

            return addedResolvedAddresses;
        }

        private static List<ResolvedAddress> CreateRandomResolvedAddresses(DateTimeOffset dateTimeOffset, bool isMatched)
        {
            return CreateResolvedAddressFiller(dateTimeOffset, isMatched)
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(DateTimeOffset dateTimeOffset, bool isMatched)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user)
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }

        private static List<ResolvedAddress> CreateRandomUnmatchedAddresses(int count)
        {
            var fillers = Enumerable.Range(1, count)
                                    .Select(_ => CreateUnmatchedAddressFiller())
                                    .ToList();

            var result = fillers.Select(filler => filler.Create()).ToList();

            return result.ToList();
        }

        private static Filler<ResolvedAddress> CreateUnmatchedAddressFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(resolvedAddress => resolvedAddress.IsProcessed).Use(false)
                .OnProperty(resolvedAddress => resolvedAddress.IsProcessing).Use(false)
                .OnProperty(resolvedAddress => resolvedAddress.IsExported).Use(false)
                .OnProperty(resolvedAddress => resolvedAddress.RetryCount).Use(3)
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user);

            return filler;
        }

        private static AssignAddress CreateRandomAssignAddress(DateTimeOffset dateTimeOffset) =>
            CreateAssignAddressFiller(dateTimeOffset).Create();

        private static Filler<AssignAddress> CreateAssignAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<AssignAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }

        private static Address? CreateRandomAddress(DateTimeOffset dateTimeOffset, string UPRN) =>
             CreateAddressFiller(dateTimeOffset, UPRN).Create();

        private static Filler<Address?> CreateAddressFiller(DateTimeOffset dateTimeOffset, string UPRN)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Address?>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.UPRN).Use(UPRN)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        public static ResolvedAddress MapOrdananceWithAssign(
            ResolvedAddress unMatchedResolvedAddress,
            AssignAddress foundAssignAddress,
            Address foundOrdananceAddress)
        {
            ResolvedAddress updatedResolovedAddress = unMatchedResolvedAddress;
            updatedResolovedAddress.UPSN = foundOrdananceAddress.UPSN;
            updatedResolovedAddress.OrganisationName = foundOrdananceAddress.OrganisationName;
            updatedResolovedAddress.DepartmentName = foundOrdananceAddress.DepartmentName;
            updatedResolovedAddress.SubBuildingName = foundOrdananceAddress.SubBuildingName;
            updatedResolovedAddress.BuildingName = foundOrdananceAddress.BuildingName;
            updatedResolovedAddress.BuildingNumber = foundOrdananceAddress.BuildingNumber;
            updatedResolovedAddress.DependentThoroughfare = foundOrdananceAddress.DependentThoroughfare;
            updatedResolovedAddress.Thoroughfare = foundOrdananceAddress.Thoroughfare;
            updatedResolovedAddress.DoubleDependentLocality = foundOrdananceAddress.DoubleDependentLocality;
            updatedResolovedAddress.DependentLocality = foundOrdananceAddress.DependentLocality;
            updatedResolovedAddress.PostTown = foundOrdananceAddress.PostTown;
            updatedResolovedAddress.PostCode = foundOrdananceAddress.PostCode;
            updatedResolovedAddress.AddressFormatQuality = foundAssignAddress.AddressFormat;
            updatedResolovedAddress.PostCodeQuality = foundAssignAddress.PostcodeQuality;
            updatedResolovedAddress.MatchedWithAssign = foundAssignAddress.Matched;
            updatedResolovedAddress.Qualifier = foundAssignAddress.Qualifier;
            updatedResolovedAddress.Classification = foundAssignAddress.Classification;
            updatedResolovedAddress.Algorithm = foundAssignAddress.Algorithm;
            updatedResolovedAddress.MatchPattern = foundAssignAddress.Pattern;
            updatedResolovedAddress.IsProcessing = true;
            updatedResolovedAddress.IsExported = false;
            updatedResolovedAddress.RetryCount = 0;

            return updatedResolovedAddress;
        }
    }
}