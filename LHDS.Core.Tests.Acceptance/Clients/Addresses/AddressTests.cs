// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    [Collection(nameof(CoreTestCollection))]
    public partial class AddressTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;
        private readonly IResolvedAddressProcessingService resolvedAddressProcessingService;
        private readonly IAddressClient addressClient;
        private readonly ICompareLogic compareLogic;
        private readonly AddressConfiguration addressConfiguration;
        private readonly BlobContainers blobContainers;

        public AddressTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddTransient<IAddressExtractionOrchestrationService, AddressExtractionOrchestrationService>()
                .AddTransient<IAddressPersistanceOrchestrationService, AddressPersistanceOrchestrationService>()
                .AddTransient<IResolvedAddressOrchestrationService, ResolvedAddressOrchestrationService>()
                .AddTransient<IResolvedAddressProcessingService, ResolvedAddressProcessingService>();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection.AddAddressClient(this.dependencyBroker.Configuration, true);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.addressExtractionOrchestrationService =
                serviceProvider.GetService<IAddressExtractionOrchestrationService>();

            this.addressPersistanceOrchestrationService =
                    serviceProvider.GetService<IAddressPersistanceOrchestrationService>();

            this.resolvedAddressOrchestrationService =
                serviceProvider.GetService<IResolvedAddressOrchestrationService>();

            this.resolvedAddressProcessingService =
                serviceProvider.GetService<IResolvedAddressProcessingService>();

            this.addressConfiguration = serviceProvider.GetService<AddressConfiguration>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
            addressClient = serviceProvider.GetService<IAddressClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static IQueryable<Address> CreateRandomAddresses()
        {
            return CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Address CreateRandomAddress(DateTimeOffset dateTimeOffset) =>
            CreateAddressFiller(dateTimeOffset).Create();

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        private static List<ResolvedAddress> CreateRandomResolvedAddresses()
        {
            return CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static ResolvedAddress CreateRandomResolvedAddress() =>
            CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ResolvedAddress CreateRandomResolvedAddress(DateTimeOffset dateTimeOffset) =>
            CreateResolvedAddressFiller(dateTimeOffset).Create();

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user);

            return filler;
        }
    }
}
