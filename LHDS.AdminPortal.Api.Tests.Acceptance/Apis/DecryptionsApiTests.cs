// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Brokers.Decryptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DecryptionsApiTests
    {
        private readonly ApiBroker apiBroker;
        private readonly ICryptographyBroker cryptographyBroker;

        public DecryptionsApiTests(
            ApiBroker apiBroker,
            ICryptographyBroker cryptographyBroker)
        {
            this.apiBroker = apiBroker;
            this.cryptographyBroker = cryptographyBroker;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(
            Guid supplierId,
            Guid subscriberCredentialId,
            string encryptedFilePath,
            string decryptedFilePath)
        {
            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(supplierId, subscriberCredentialId, encryptedFilePath, decryptedFilePath);

            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private static IngestionTracking CreateRandomIngestionTracking(
            Guid supplierId,
            Guid subscriberCredentialId,
            string encryptedFilePath,
            string decryptedFilePath) =>
            CreateRandomIngestionTrackingFiller(
                supplierId, subscriberCredentialId, encryptedFilePath, decryptedFilePath).Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller(
            Guid supplierId,
            Guid subscriberCredentialId,
            string encryptedFilePath,
            string decryptedFilePath)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            string fileName = $"{GetRandomString()}/{GetRandomString()}/{subscriberCredentialId}/{supplierId}";
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use($"{fileName}.doc")

                .OnProperty(ingestionTracking =>
                    ingestionTracking.EncryptedFileName).Use($"/{encryptedFilePath}/{fileName}.doc")

                .OnProperty(ingestionTracking =>
                    ingestionTracking.DecryptedFileName).Use($"/{decryptedFilePath}/{fileName}.doc")

                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);
            return filler;
        }

        private async ValueTask<Supplier> PostRandomSupplierAsync()
        {
            Supplier randomSupplier = CreateRandomSupplier();
            await this.apiBroker.PostSupplierAsync(randomSupplier);

            return randomSupplier;
        }

        private static Supplier CreateRandomSupplier() =>
            CreateRandomSupplierFiller().Create();

        private static Filler<Supplier> CreateRandomSupplierFiller()
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }

        private async ValueTask DeleteAuditRecordsAsync(IngestionTracking inputIngestionTracking)
        {
            var audits = await this.apiBroker.FindIngestionTrackingAuditByIngestionTrackingIdAsync(
                inputIngestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(audit.Id);
            }
        }

        private static SubscriberCredential CreateRandomSubscriberCredential(Guid id) =>
            CreateRandomSubscriberCredentialFiller(id).Create();

        private static Filler<SubscriberCredential> CreateRandomSubscriberCredentialFiller(Guid id)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(id)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }
    }
}