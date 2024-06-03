// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
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
        private readonly IDocumentService documentService;
        private readonly IAddressService addressService;
        private readonly IResolvedAddressService resolvedAddressService;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IAddressClient addressClient;
        private readonly ICompareLogic compareLogic;
        private readonly IDateTimeBroker dateTimeBroker;
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
                .AddTransient<IResolvedAddressProcessingService, ResolvedAddressProcessingService>()
                .AddTransient<IDocumentService, DocumentService> ()
                .AddTransient<ICsvHelperBroker, CsvHelperBroker> ()
                .AddTransient<IAddressService, AddressService>()
                .AddTransient<IDocumentService, DocumentService>();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection.AddAddressClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.addressExtractionOrchestrationService =
                serviceProvider.GetService<IAddressExtractionOrchestrationService>();

            this.addressPersistanceOrchestrationService =
                    serviceProvider.GetService<IAddressPersistanceOrchestrationService>();

            this.resolvedAddressOrchestrationService =
                serviceProvider.GetService<IResolvedAddressOrchestrationService>();

            this.addressService = serviceProvider.GetService<IAddressService>();
            this.resolvedAddressService = serviceProvider.GetService<IResolvedAddressService>();

            this.resolvedAddressProcessingService =
                serviceProvider.GetService<IResolvedAddressProcessingService>();

            this.documentService =
                serviceProvider.GetService<IDocumentService>();

            this.csvHelperBroker =
                serviceProvider.GetService<ICsvHelperBroker>();

            this.addressConfiguration = serviceProvider.GetService<AddressConfiguration>();
            this.blobContainers = serviceProvider.GetService<BlobContainers>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            addressClient = serviceProvider.GetService<IAddressClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static List<dynamic> GetDynamicRandomAddresses()
        {
            List<dynamic> dynamicAddresses = new List<dynamic>();

            dynamic address1 = new
            {
                UPSN = "upsn",
                UPRN = "uprn",
                OrganisationName = "",
                DepartmentName = "",
                SubBuildingName = "",
                BuildingName = "",
                BuildingNumber = "10",
                DependentThoroughfare = "",
                Thoroughfare = "Downing Str",
                DoubleDependentLocality = "",
                DependentLocality = "Westminister",
                PostTown = "London",
                PostCode = "SW1A2AA",
                PostalAddress = "10 Downing Str, Westminster, London, SW1A2AA, UK",
                JsonPostalAddress = "{\"house_number\":\"10\",\"road\":\"Downing Str\",\"city_district\":\"Westminister\",\"city\":\"London\",\"postcode\":\"SW1A2AA\",\"country\":\"UK\"}",
                UnstructuredPostalAddress = "10 downing str westminster london sw1a2aa uk"
            };

            dynamicAddresses.Add(address1);

            dynamic address2 = new
            {
                UPSN = "upsn",
                UPRN = "uprn",
                OrganisationName = "",
                DepartmentName = "",
                SubBuildingName = "",
                BuildingName = "",
                BuildingNumber = "9",
                DependentThoroughfare = "",
                Thoroughfare = "The Woodfields",
                DoubleDependentLocality = "",
                DependentLocality = "Sanderstead",
                PostTown = "London",
                PostCode = "CR2 0HG",
                PostalAddress = "9 The Woodfields, Sanderstead, London, CR2 0HG, UK",
                JsonPostalAddress = "{\"house_number\":\"9\",\"road\":\"The Woodfields\",\"city_district\":\"Sanderstead\",\"city\":\"London\",\"postcode\":\"CR2 0HG\",\"country\":\"UK\"}",
                UnstructuredPostalAddress = "9 the woodfields sanderstead london cr2 0hg uk"
            };

            dynamicAddresses.Add(address2);

            dynamic address3 = new
            {
                UPSN = "upsn",
                UPRN = "uprn",
                OrganisationName = "",
                DepartmentName = "",
                SubBuildingName = "",
                BuildingName = "",
                BuildingNumber = "",
                DependentThoroughfare = "Edinburgh Castle",
                Thoroughfare = "Castlehill",
                DoubleDependentLocality = "",
                DependentLocality = "",
                PostTown = "Edinburgh",
                PostCode = "EH1 2NG",
                PostalAddress = "Edinburgh Castle, Castlehill, Edinburgh, EH1 2NG, UK",
                JsonPostalAddress = "{\"house_number\":\"9\",\"road\":\"Castlehill\",\"city\":\"Edinburgh\",\"postcode\":\"EH1 2NG\",\"country\":\"UK\"}",
                UnstructuredPostalAddress = "edinburgh castle castlehill edinburgh eh1 2ng uk"
            };

            dynamicAddresses.Add(address3);

            return dynamicAddresses;
        }


        private static IQueryable<Address> CreateRandomAddresses(int randomCount, DateTimeOffset dateTimeOffset)
        {
            //var postcodes = new List<string> { "B12 9XY", "CR2 9PA", "CR2 0HG", };
            var postcodes = new List<string> { "B12 9XY" };
            return CreateAddressFiller(dateTimeOffset, postcodes)
                .Create(count: randomCount)
                    .AsQueryable();
        }

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset, List<string> postcodes)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user)
                .OnProperty(address => address.PostalAddress).Use("9 The Woodfields, Sanderstead, Surrey, CR20HG")
                .OnProperty(address => address.JsonPostalAddress).Use("{\"house_number\":\"789\",\"road\":\"david road\",\"city\":\"birmingham\",\"postcode\":\"b12 9xy\",\"state\":\"england\"}")
                .OnProperty(address => address.PostCode).Use(() => GetRandomPostcode(postcodes));

            return filler;
        }

        private static string GetRandomPostcode(List<string> postcodes)
        {
            Random rand = new Random();
            int index = rand.Next(postcodes.Count);
            return postcodes[index];
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
                .OnProperty(resolvedAddress => resolvedAddress.IsMatched).Use(isMatched)
                .OnProperty(resolvedAddress => resolvedAddress.IsProcessed).Use(false)
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user)
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}
