// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.KeyVaults;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Decryptions
{
    [Collection(nameof(CoreTestCollection))]
    public partial class DecryptionTests
    {
        private readonly DependencyBroker dependencyBroker;
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<IDownloadBroker> downloadBrokerMock;
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDecryptionClient decryptionClient;
        private readonly LandingConfiguration landingConfiguration;
        private readonly ICryptographyProvider cryptographyProvider;
        private readonly IIngestionTrackingAuditService auditService;

        public DecryptionTests(DependencyBroker dependencyBroker)
        {
            this.dependencyBroker = dependencyBroker;
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.downloadBrokerMock = new Mock<IDownloadBroker>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            var blobStorageSettings =
                this.dependencyBroker.Configuration.GetSection("blobStorage").Get<BlobStorageSettings>();

            ValidateBlobStorageSettings(blobStorageSettings);
            serviceCollection.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);

            serviceCollection.AddDecryptionClientForAcceptance(this.dependencyBroker.Configuration);

            serviceCollection.AddTransient<IKeyVaultSecretBroker>((LandingConfiguration) =>
                    new KeyVaultSecretBroker(landingConfiguration.KeyVaultUrl));

            serviceCollection
                .AddTransient<IDownloadBroker>(serviceProvider => downloadBrokerMock.Object)
                .AddTransient<IBlobStorageBroker>(serviceProvider => blobStorageBrokerMock.Object);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            this.ingestionTrackingService = serviceProvider.GetService<IIngestionTrackingService>();
            this.auditService = serviceProvider.GetService<IIngestionTrackingAuditService>();
            this.dateTimeBroker = serviceProvider.GetService<IDateTimeBroker>();
            this.storageBroker = serviceProvider.GetService<IStorageBroker>();
            this.landingConfiguration = serviceProvider.GetRequiredService<LandingConfiguration>();
            this.cryptographyProvider = serviceProvider.GetRequiredService<ICryptographyProvider>();
            decryptionClient = serviceProvider.GetService<IDecryptionClient>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
          new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

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

        private static SubscriberAgreement CreateRandomSubscriberAgreement(DateTimeOffset dateTimeOffset) =>
           CreateSubscriberAgreementFiller(dateTimeOffset).Create();

        private static Filler<SubscriberAgreement> CreateSubscriberAgreementFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberAgreement>();
            Guid supplierSharingAgreementGuid = new Guid("6263EBC7-D8CC-4AA9-8849-60DCEDB63974");

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)

                .OnProperty(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementGuid)
                    .Use(supplierSharingAgreementGuid)

                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }

        private static string GetRandomMessage() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();
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

        private static SubscriberCredential CreateRandomSubscriberCredential() =>
           CreateSubscriberCredentialFiller().Create();

        private static Filler<SubscriberCredential> CreateSubscriberCredentialFiller()
        {
            var filler = new Filler<SubscriberCredential>();
            string user = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.GpgPublicKey)
                .Use("LS0tLS1CRUdJTiBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0tCgptRE1FWStKTjl4WUpLd1lCQkFIYVJ3OEJBUWRBZFpNbEhySm9yRDJSaDJlTGFNRDJOUFNUeW9mYzRXVnU2R1FhCndNNWd5dWUwRmt4SVJGTWdQSFJsYzNSQWJHaGtjeTVzYjJOaGJENklrd1FURmdvQU94WWhCQkF4dVR3MnBHQjcKOHBFSWFsVjJadU1ZSW9PbEJRSmo0azMzQWhzREJRc0pDQWNDQWlJQ0JoVUtDUWdMQWdRV0FnTUJBaDRIQWhlQQpBQW9KRUZWMlp1TVlJb09sVmw0QS8xRk8xVUd3V05JcWFuUTBPaDJKbTlQK2YyNVRvWFJ2bnRRRktTYUx2aUJZCkFQNDhCQ3FaVm1pcERaaFlNdVFmUWpyTmJEV2gvUWc1YTlJUG5xeHB4N2JtQUxnNEJHUGlUZmNTQ2lzR0FRUUIKbDFVQkJRRUJCMEFCUXl0b3hoNExMSCtPRi9OdmhxamE1THNtbEhPNS9wTU5rUnlSZ0l2eEZRTUJDQWVJZUFRWQpGZ29BSUJZaEJCQXh1VHcycEdCNzhwRUlhbFYyWnVNWUlvT2xCUUpqNGszM0Foc01BQW9KRUZWMlp1TVlJb09sCk5OZ0JBTEdSWWNFbzRYeXJJZjVaaDhSeWRNd1dqVTJtZjk5M0tZWjFQaVgyLy9hbUFQOVF4WnNac2oxTjJNdDAKTjZveWs3UDJSRExrSTBVRDZiUlJTS0NTVnFIRkJBPT0KPWVIMEoKLS0tLS1FTkQgUEdQIFBVQkxJQyBLRVkgQkxPQ0stLS0tLQo=")
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }

        private static string CreateRandomFilePath(Guid? identifier)
        {
            return $"{GetRandomString()}/{GetRandomString()}" +
                $"/{identifier}/0122235/{GetRandomNumber}" +
                $"_{GetRandomString()}_{GetRandomString()}" +
                $"_{GetRandomNumber()}_{identifier}.csv.gpg;";
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings blobStorageSettings)
        {
            if (blobStorageSettings == null)
            {
                throw new InvalidConfigurationException("Configuration section 'blobStorage' not defined.");
            }

            Validate(
                (Rule: IsInvalid(blobStorageSettings.AzureBlobServiceUri),
                    Parameter: "blobStorage__azureBlobServiceUri"),

                (Rule: IsInvalid(blobStorageSettings.AzureTenantId),
                    Parameter: "blobStorage__azureTenantId"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            StringBuilder validationErrors = new StringBuilder();
            validationErrors.AppendLine("Configuration error(s):");
            IDictionary errors = new Dictionary<string, List<string>>();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    validationErrors.AppendLine(
                        $"{parameter} -> Configuration value does not exist or does not meet validation criteria");

                    if (errors.Contains(parameter))
                    {
                        (errors[parameter] as List<string>)?.Add(rule.Message);
                        return;
                    }

                    errors.Add(parameter, new List<string> { rule.Message });
                }
            }

            var invalidConfigurationException = new InvalidConfigurationException(
                message: validationErrors.ToString(),
                data: errors);

            invalidConfigurationException.ThrowIfContainsErrors();
        }
    }
}
