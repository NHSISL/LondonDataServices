// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
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
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;
        private readonly ITestOutputHelper output;

        public IngestionTrackingProcessingServiceTests(ITestOutputHelper output)
        {
            this.output = output;

            ingestionTrackingProcessingService = new IngestionTrackingProcessingService(
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
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

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking exprectedIngestionTracking)
        {
            return actualIngestionTracking =>
                IsSameIngestionTracking(exprectedIngestionTracking, actualIngestionTracking);
        }

        private Expression<Func<List<IngestionTracking>, bool>> SameIngestionTrackingsAs(
            List<IngestionTracking> expectedIngestionTrackings)
        {
            return actualIngestionTrackings =>
                IsSameIngestionTrackings(expectedIngestionTrackings, actualIngestionTrackings);
        }

        private bool IsSameIngestionTracking(
            IngestionTracking expectedIngestionTracking,
            IngestionTracking actualIngestionTracking)
        {
            try
            {
                actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
            }
            catch (Exception exception)
            {
                output.WriteLine(exception.Message);
            }

            return new CompareLogic().Compare(expectedIngestionTracking, actualIngestionTracking).AreEqual;
        }

        private bool IsSameIngestionTrackings(
            List<IngestionTracking> expectedIngestionTrackings,
            List<IngestionTracking> actualIngestionTrackings)
        {
            try
            {
                actualIngestionTrackings.Should().BeEquivalentTo(expectedIngestionTrackings);
            }
            catch (Exception exception)
            {
                output.WriteLine(exception.Message);
            }

            return new CompareLogic().Compare(expectedIngestionTrackings, actualIngestionTrackings).AreEqual;
        }

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
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreement).IgnoreIt();

            return filler;
        }
    }
}
