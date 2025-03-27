// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberCredentials
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SubscriberCredentialsApiTests
    {
        private readonly ApiBroker apiBroker;

        public SubscriberCredentialsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private List<Guid> CreateRandomSubscriberAgreementIds()
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberAgreementIds = new List<Guid>();

            for (int i = 0; i < randomNumber; i++)
            {
                Guid id = Guid.NewGuid();
                randomSubscriberAgreementIds.Add(id);
            }

            return randomSubscriberAgreementIds;
        }

        private static SubscriberCredential UpdateSubscriberCredentialWithRandomValues(
            SubscriberCredential inputSubscriberCredential)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnProperty(subscriberCredential => subscriberCredential.Id)
                    .Use(inputSubscriberCredential.Id)

                .OnType<DateTimeOffset>().Use(GetRandomDateTime())

                .OnProperty(subscriberCredential => subscriberCredential.CreatedDate)
                    .Use(inputSubscriberCredential.CreatedDate)

                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy)
                    .Use(inputSubscriberCredential.CreatedBy)

                .OnProperty(subscriberCredential => subscriberCredential.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<SubscriberCredential> PostRandomSubscriberCredentialAsync(Guid id)
        {
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(id);
            await this.apiBroker.PostSubscriberCredentialAsync(randomSubscriberCredential);

            return randomSubscriberCredential;
        }

        private async ValueTask<List<SubscriberCredential>> PostRandomSubscriberCredentialsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberCredentials = new List<SubscriberCredential>();

            for (int i = 0; i < randomNumber; i++)
            {
                Guid id = Guid.NewGuid();
                randomSubscriberCredentials.Add(await PostRandomSubscriberCredentialAsync(id));
            }

            return randomSubscriberCredentials;
        }

        public static List<SubscriberCredential> CreatSubscriberCredentialsFromAgreements(
            List<SubscriberAgreement> subscriberAgreements)
        {
            List<SubscriberCredential> subscriberCredentials = new List<SubscriberCredential>();

            foreach (SubscriberAgreement subscriberAgreement in subscriberAgreements)
            {
                SubscriberCredential subscriberCredential =
                    CreateSubscriberCredentialFromAgreement(subscriberAgreement);

                subscriberCredentials.Add(subscriberCredential);
            }

            return subscriberCredentials;
        }

        private static SubscriberCredential CreateSubscriberCredentialFromAgreement(
            SubscriberAgreement subscriberAgreement)
        {
            SubscriberCredential subscriberCredential = new SubscriberCredential
            {
                Id = subscriberAgreement.Id,
                SupplierSharingAgreementShortName = subscriberAgreement.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = subscriberAgreement.SupplierSharingAgreementGuid,
                FtpPublicKey = subscriberAgreement.FtpPublicKey,
                FtpUserName = subscriberAgreement.FtpUserName,
                FtpPassPhrase = null,
                FtpPassword = null,
                FtpPrivateKey = null,
                GpgPublicKey = subscriberAgreement.GpgPublicKey,
                GpgPassPhrase = null,
                GpgPrivateKey = null,
                IsActive = subscriberAgreement.IsActive,
                LastPollEndDate = subscriberAgreement.LastPollEndDate,
                LastPollStartDate = subscriberAgreement.LastPollStartDate,
                CreatedBy = subscriberAgreement.CreatedBy,
                CreatedDate = subscriberAgreement.CreatedDate,
                UpdatedBy = subscriberAgreement.UpdatedBy,
                UpdatedDate = subscriberAgreement.UpdatedDate,
            };

            return subscriberCredential;
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

        private async ValueTask<SubscriberAgreement> PostRandomSubscriberAgreementAsync(Guid id)
        {
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(id);

            SubscriberAgreement storageSubscriberAgreement = 
                await this.apiBroker.PostSubscriberAgreementAsync(randomSubscriberAgreement);

            return storageSubscriberAgreement;
        }

        private async ValueTask<List<SubscriberAgreement>> PostRandomSubscriberAgreementsAsync(
            List<Guid> subscriberAgreementIds)
        {
            var randomSubscriberAgreements = new List<SubscriberAgreement>();

            foreach (Guid subscriberAgreementId in subscriberAgreementIds)
            {
                randomSubscriberAgreements.Add(await PostRandomSubscriberAgreementAsync(subscriberAgreementId));
            }

            return randomSubscriberAgreements;
        }

        public static List<SubscriberAgreement> CreateRandomSubscriberAgreements()
        {
            List<SubscriberAgreement> subscriberAgreements = new List<SubscriberAgreement>();

            for (int i = 0; i < 10; i++)
            {
                Guid id = Guid.NewGuid();

                SubscriberAgreement subscriberAgreement = CreateRandomSubscriberAgreement(id);

                subscriberAgreements.Add(subscriberAgreement);
            }

            return subscriberAgreements;
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement(Guid id) =>
            CreateRandomSubscriberAgreementFiller(id).Create();

        private static Filler<SubscriberAgreement> CreateRandomSubscriberAgreementFiller(Guid id)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.Id).Use(id)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }
    }
}