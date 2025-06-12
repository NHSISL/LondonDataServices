// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberAgreements
{
    public partial class SubscriberAgreementControllerTests : RESTFulController
    {
        private readonly Mock<ISubscriberAgreementService> subscriberAgreementServiceMock;
        private readonly SubscriberAgreementsController subscriberAgreementsController;

        public SubscriberAgreementControllerTests()
        {
            this.subscriberAgreementServiceMock = new Mock<ISubscriberAgreementService>();

            this.subscriberAgreementsController = new SubscriberAgreementsController(
                this.subscriberAgreementServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<SubscriberAgreement> CreateRandomSubscriberAgreements() =>
            CreateSubscriberAgreementFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static SubscriberAgreement CreateRandomSubscriberAgreement() =>
            CreateSubscriberAgreementFiller().Create();

        private static Filler<SubscriberAgreement> CreateSubscriberAgreementFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(accessAudit => accessAudit.CreatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.UpdatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.IngestionTrackings).IgnoreIt();

            return filler;
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new SubscriberAgreementDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new SubscriberAgreementServiceException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }

        public static TheoryData<Xeption> ValidationExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new SubscriberAgreementValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new SubscriberAgreementDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
