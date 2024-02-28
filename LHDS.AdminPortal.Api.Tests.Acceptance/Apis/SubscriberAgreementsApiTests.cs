// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberAgreements
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SubscriberAgreementsApiTests
    {
        private readonly ApiBroker apiBroker;

        public SubscriberAgreementsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SubscriberAgreement UpdatSubscriberAgreementWithRandomValues(SubscriberAgreement inputSubscriberAgreement)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnProperty(SubscriberAgreement => SubscriberAgreement.Id).Use(inputSubscriberAgreement.Id)

                .OnProperty(SubscriberAgreement => SubscriberAgreement.SupplierSharingAgreementShortName)
                    .Use(inputSubscriberAgreement.SupplierSharingAgreementShortName)

                .OnProperty(SubscriberAgreement => SubscriberAgreement.SupplierSharingAgreementGuid)
                    .Use(inputSubscriberAgreement.SupplierSharingAgreementGuid)

                .OnProperty(SubscriberAgreement => SubscriberAgreement.FtpPublicKey).Use(inputSubscriberAgreement.FtpPublicKey)
                .OnProperty(SubscriberAgreement => SubscriberAgreement.FtpUserName).Use(inputSubscriberAgreement.FtpUserName)
                .OnProperty(SubscriberAgreement => SubscriberAgreement.GpgPublicKey).Use(inputSubscriberAgreement.GpgPublicKey)
                .OnProperty(SubscriberAgreement => SubscriberAgreement.CreatedBy).Use(inputSubscriberAgreement.CreatedBy)
                .OnProperty(SubscriberAgreement => SubscriberAgreement.CreatedDate).Use(inputSubscriberAgreement.CreatedDate)
                .OnProperty(SubscriberAgreement => SubscriberAgreement.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static IQueryable<SubscriberAgreement> CreateRandomSubscriberAgreements()
        {
            return CreateSubscriberAgreementFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement() =>
            CreateSubscriberAgreementFiller().Create();

        private static Filler<SubscriberAgreement> CreateSubscriberAgreementFiller()
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberAgreement>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }
    }
}