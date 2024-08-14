// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        private readonly Mock<IIngestionTrackingService> ingestionTrackingServiceMock =
            new Mock<IIngestionTrackingService>();

        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;

        public IngestionTrackingProcessingServiceTests()
        {
            ingestionTrackingProcessingService = new IngestionTrackingProcessingService(
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new IngestionTrackingValidationException(
                    message: "IngestionTracking validation errors occurred, please try again.", innerException),

                new IngestionTrackingDependencyValidationException(
                    message: "IngestionTracking dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new IngestionTrackingDependencyException(
                    message: "IngestionTracking validation errors occurred, please try again.", innerException),

                new IngestionTrackingServiceException(
                    message : "IngestionTracking service error occurred, please contact support.", innerException)
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static List<IngestionTracking> CreateRandomIngestionTrackings()
        {
            return CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

            return filler;
        }
    }
}
