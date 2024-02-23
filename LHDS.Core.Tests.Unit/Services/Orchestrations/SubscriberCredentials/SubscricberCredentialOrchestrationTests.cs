// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SubscriberAgreements;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        private readonly Mock<ISubscriberAgreementProcessingService> subscriberAgreementProcessingServiceMock;
        private readonly Mock<ISecureDataProcessingService> secureDataProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;

        public SubscriberCredentialOrchestrationServiceTests()
        {
            this.subscriberAgreementProcessingServiceMock = new Mock<ISubscriberAgreementProcessingService>();
            this.secureDataProcessingServiceMock = new Mock<ISecureDataProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.subscriberCredentialOrchestration = new SubscriberCredentialOrchestration(
                subscriberAgreementProcessingService: subscriberAgreementProcessingServiceMock.Object,
                secureDataProcessingService: secureDataProcessingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
           new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<AddressExtractionAudit, bool>> SameAddressExtractionAuditAs(
            AddressExtractionAudit expectedAddressExtractionAudit)
        {
            return actualAddressExtractionAudit =>
                this.compareLogic.Compare(expectedAddressExtractionAudit, actualAddressExtractionAudit)
                    .AreEqual;
        }

        private static SubscriberCredential CreateRandomCreateSubscriberCredential(DateTimeOffset dateTimeOffset) =>
            CreateSubscriberCredentialFiller(dateTimeOffset).Create();

        private static Filler<SubscriberCredential> CreateSubscriberCredentialFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }
    }
}
