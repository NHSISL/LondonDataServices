// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    [Collection(nameof(CoreTestCollection))]
    public partial class DecryptionTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDocumentService documentService;
        private readonly IDecryptionClient decryptionClient;
        private readonly LandingConfiguration landingConfiguration;
        private readonly ICryptographyProvider cryptographyProvider;
        private readonly IAuditService auditService;

        public DecryptionTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            serviceCollection.AddDecryptionClient(this.dependencyBroker.Configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            this.documentService = serviceProvider.GetService<IDocumentService>();
            this.auditService = serviceProvider.GetService<IAuditService>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            this.landingConfiguration = serviceProvider.GetRequiredService<LandingConfiguration>();
            this.cryptographyProvider = serviceProvider.GetRequiredService<ICryptographyProvider>();
            decryptionClient = serviceProvider.GetService<IDecryptionClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static IngestionTracking CreateRandomIngestionTracking(
           DateTimeOffset dateTimeOffset,
           string fileName,
           string encryptedFileName,
           string decryptedFileName,
           Guid supplierId)
        {
            IngestionTracking ingestionTracking = CreateIngestionTrackingFiller(
                dateTimeOffset,
                fileName: fileName,
                encryptedFileName,
                decryptedFileName,
                supplierId)
                    .Create();

            return ingestionTracking;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset,
            string fileName,
            string encryptedFileName,
            string decryptedFileName,
            Guid supplierId)
        {
            string user = "System";
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(fileName)
                .OnProperty(ingestionTracking => ingestionTracking.EncryptedFileName).Use(encryptedFileName)
                .OnProperty(ingestionTracking => ingestionTracking.DecryptedFileName).Use(decryptedFileName)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }
    }
}
