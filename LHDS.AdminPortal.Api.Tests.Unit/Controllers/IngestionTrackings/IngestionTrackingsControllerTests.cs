// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackings
{
    public partial class IngestionTrackingsControllerTests : RESTFulController
    {
        private readonly Mock<IIngestionTrackingService> ingestionTrackingServiceMock;
        private readonly IngestionTrackingsController ingestionTrackingsController;

        public IngestionTrackingsControllerTests()
        {
            this.ingestionTrackingServiceMock = new Mock<IIngestionTrackingService>();

            this.ingestionTrackingsController = new IngestionTrackingsController(
                this.ingestionTrackingServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<IngestionTracking> CreateRandomIngestionTrackings() =>
            CreateIngestionTrackingFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateIngestionTrackingFiller().Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreement).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

            return filler;
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new IngestionTrackingDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new IngestionTrackingServiceException(
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
                new IngestionTrackingValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new IngestionTrackingDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
