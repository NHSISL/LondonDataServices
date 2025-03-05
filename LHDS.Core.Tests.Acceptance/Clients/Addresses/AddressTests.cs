// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using WireMock.Server;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    [Collection(nameof(CoreTestCollection))]
    public partial class AddressTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly IAddressOrchestrationService addressOrchestrationService;
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
        private readonly WireMockServer wireMockServer;

        public AddressTests(DependencyBroker dependencyBroker)
        {
            this.wireMockServer = WireMockServer.Start();
            this.dependencyBroker = dependencyBroker;
            this.compareLogic = new CompareLogic();
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddTransient<IAddressOrchestrationService, AddressOrchestrationService>()
                .AddTransient<IResolvedAddressOrchestrationService, ResolvedAddressOrchestrationService>()
                .AddTransient<IResolvedAddressProcessingService, ResolvedAddressProcessingService>()
                .AddTransient<IDocumentService, DocumentService>()
                .AddTransient<ICsvHelperBroker, CsvHelperBroker>()
                .AddTransient<IAddressService, AddressService>()
                .AddTransient<IDocumentService, DocumentService>()
                .AddSingleton<IStorageBroker, StorageBroker>();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            this.dependencyBroker.Configuration["assignConfiguration:apiUrl"] = this.wireMockServer.Url;
            serviceCollection.AddAddressClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.addressOrchestrationService =
                serviceProvider.GetService<IAddressOrchestrationService>();

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

        static byte[] ReadAllBytesFromStream(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
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

        private static Address GetRandomAddress()
        {
            DateTimeOffset randomDateTime = DateTime.UtcNow;

            Address address = new Address
            {
                Id = Guid.NewGuid(),
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
                IsProcessing = false,
                IsSynced = false,
                CreatedBy = "system",
                UpdatedBy = "system",
                CreatedDate = randomDateTime,
                UpdatedDate = randomDateTime
            };

            return address;
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

        private static Address CreateRandomAddress(DateTimeOffset dateTimeOffset, string UPRN) =>
             CreateAddressFiller(dateTimeOffset, UPRN).Create();

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset, string UPRN)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.UPRN).Use(UPRN)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
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
                .OnProperty(address => address.PostCode).Use(() => GetRandomPostcode(postcodes));

            return filler;
        }

        public static ResolvedAddress MapOrdananceWithAssign(
            ResolvedAddress unMatchedResolvedAddress,
            AssignAddress foundAssignAddress,
            Address foundOrdananceAddress)
        {
            ResolvedAddress updatedResolovedAddress = unMatchedResolvedAddress;
            updatedResolovedAddress.UPRN = foundOrdananceAddress.UPRN;
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
            updatedResolovedAddress.Qualifier = foundAssignAddress.BestMatch.Qualifier;
            updatedResolovedAddress.Classification = foundAssignAddress.BestMatch.Classification;
            updatedResolovedAddress.Algorithm = foundAssignAddress.BestMatch.Algorithm;
            updatedResolovedAddress.MatchPattern = foundAssignAddress.Pattern;
            updatedResolovedAddress.IsProcessing = true;
            updatedResolovedAddress.IsExported = false;
            updatedResolovedAddress.RetryCount = 0;

            return updatedResolovedAddress;
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
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user)
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }

        private async ValueTask<string> MapObjectToCsv(List<ResolvedAddress> resolvedAddresses)
        {
            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { nameof(ResolvedAddress.UniqueReference), 0 },
                { nameof(ResolvedAddress.UPRN), 1 },
                { nameof(ResolvedAddress.UPSN), 2 },
                { nameof(ResolvedAddress.OrganisationName), 3 },
                { nameof(ResolvedAddress.DepartmentName), 4 },
                { nameof(ResolvedAddress.SubBuildingName), 5 },
                { nameof(ResolvedAddress.BuildingName), 6 },
                { nameof(ResolvedAddress.BuildingNumber), 7 },
                { nameof(ResolvedAddress.DependentThoroughfare), 8 },
                { nameof(ResolvedAddress.Thoroughfare), 9 },
                { nameof(ResolvedAddress.DoubleDependentLocality), 10 },
                { nameof(ResolvedAddress.DependentLocality), 11 },
                { nameof(ResolvedAddress.PostTown), 12 },
                { nameof(ResolvedAddress.PostCode), 13 },
                { nameof(ResolvedAddress.AddressFormatQuality), 14 },
                { nameof(ResolvedAddress.PostCodeQuality), 15 },
                { nameof(ResolvedAddress.MatchedWithAssign), 16 },
                { nameof(ResolvedAddress.Qualifier), 17 },
                { nameof(ResolvedAddress.Classification), 18 },
                { nameof(ResolvedAddress.Algorithm), 19 },
                { nameof(ResolvedAddress.MatchPattern), 20 },
                { nameof(ResolvedAddress.UnstructuredPostalAddress), 21 }
            };

            return await this.csvHelperBroker
               .MapObjectToCsvAsync(
                    @object: resolvedAddresses,
                    addHeaderRecord: true,
                    fieldMappings: fieldMappings,
                    shouldAddTrailingComma: true);
        }

        private async ValueTask<List<ResolvedAddress>> MapCsvToObject(string data)
        {
            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
            {
                { nameof(ResolvedAddress.UniqueReference), 0 },
                { nameof(ResolvedAddress.UPRN), 1 },
                { nameof(ResolvedAddress.UPSN), 2 },
                { nameof(ResolvedAddress.OrganisationName), 3 },
                { nameof(ResolvedAddress.DepartmentName), 4 },
                { nameof(ResolvedAddress.SubBuildingName), 5 },
                { nameof(ResolvedAddress.BuildingName), 6 },
                { nameof(ResolvedAddress.BuildingNumber), 7 },
                { nameof(ResolvedAddress.DependentThoroughfare), 8 },
                { nameof(ResolvedAddress.Thoroughfare), 9 },
                { nameof(ResolvedAddress.DoubleDependentLocality), 10 },
                { nameof(ResolvedAddress.DependentLocality), 11 },
                { nameof(ResolvedAddress.PostTown), 12 },
                { nameof(ResolvedAddress.PostCode), 13 },
                { nameof(ResolvedAddress.AddressFormatQuality), 14 },
                { nameof(ResolvedAddress.PostCodeQuality), 15 },
                { nameof(ResolvedAddress.MatchedWithAssign), 16 },
                { nameof(ResolvedAddress.Qualifier), 17 },
                { nameof(ResolvedAddress.Classification), 18 },
                { nameof(ResolvedAddress.Algorithm), 19 },
                { nameof(ResolvedAddress.MatchPattern), 20 },
                { nameof(ResolvedAddress.UnstructuredPostalAddress), 21 }
            };

            return await this.csvHelperBroker
               .MapCsvToObjectAsync<ResolvedAddress>(
                    data,
                    hasHeaderRecord: true,
                    fieldMappings: fieldMappings);
        }
    }
}
