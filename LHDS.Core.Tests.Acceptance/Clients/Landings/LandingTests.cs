// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Landings
{
    [Collection(nameof(CoreTestCollection))]
    public partial class LandingTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<IDownloadBroker> downloadBrokerMock;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDataSetSpecificationService dataSetSpecificationService;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ILandingClient landingClient;
        private readonly LandingConfiguration landingConfiguration;
        private readonly IIngestionTrackingAuditService auditService;

        private readonly DependencyBroker dependencyBroker;

        public LandingTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.downloadBrokerMock = new Mock<IDownloadBroker>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            var blobStorageSettings = dependencyBroker.Configuration
                .GetSection("blobStorage").Get<BlobStorageSettings>();

            serviceCollection.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);

            serviceCollection
                .AddTransient<IDownloadBroker>(serviceProvider => downloadBrokerMock.Object)
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object)
                .AddTransient<IDataSetSpecificationService, DataSetSpecificationService>()
                .AddTransient<IDataSetSpecificationProcessingService, DataSetSpecificationProcessingService>();

            serviceCollection.AddLandingClientForAcceptance(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            this.dataSetSpecificationService = serviceProvider.GetService<IDataSetSpecificationService>();

            this.dataSetSpecificationProcessingService = 
                serviceProvider.GetService<IDataSetSpecificationProcessingService>();

            this.auditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            this.landingConfiguration = serviceProvider.GetService<LandingConfiguration>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            landingClient = serviceProvider.GetService<ILandingClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static IngestionTracking CreateRandomIngestionTracking(
           DateTimeOffset dateTimeOffset,
           Document document,
           Guid supplierId)
        {
            IngestionTracking ingestionTracking = CreateIngestionTrackingFiller(
                dateTimeOffset,
                fileName: document.FileName,
                supplierId)
                    .Create();

            return ingestionTracking;
        }

        private async ValueTask<List<IngestionTracking>> CreateRandomIngestionTrackings(
            DateTimeOffset dateTimeOffset,
            List<Document> documents,
            Guid supplierId)
        {
            List<IngestionTracking> items = new List<IngestionTracking>();

            foreach (var document in documents)
            {
                var item = CreateIngestionTrackingFiller(
                    dateTimeOffset,
                    fileName: document.FileName,
                    supplierId)
                        .Create();

                await this.ingestionTrackingService.AddIngestionTrackingAsync(item);
                items.Add(item);
            }

            return items;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset, string fileName, Guid supplierId)
        {
            string user = "System";
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(fileName)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }
    }
}
