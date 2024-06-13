// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        private readonly Mock<IIngestionTrackingAuditService> ingestionTrackingAuditServiceMock = new Mock<IIngestionTrackingAuditService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IIngestionTrackingAuditProcessingService ingestionTrackingAuditProcessingService;

        public IngestionTrackingAuditProcessingServiceTests()
        {
            ingestionTrackingAuditProcessingService = new IngestionTrackingAuditProcessingService(
                ingestionTrackingAuditService: ingestionTrackingAuditServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.", innerException),

                new IngestionTrackingAuditDependencyValidationException(
                    message: "IngestionTrackingAudit dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new IngestionTrackingAuditDependencyException(
                    message: "IngestionTrackingAudit dependency validation errors occurred, please try again.", innerException),

                new IngestionTrackingAuditServiceException(
                    message : "IngestionTrackingAudit service error occurred, please contact support.", innerException)
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

        private static IngestionTrackingAudit CreateRandomIngestionTrackingAudit() =>
            CreateIngestionTrackingAuditFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static IQueryable<IngestionTrackingAudit> CreateRandomIngestionTrackingAudits()
        {
            return CreateIngestionTrackingAuditFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<IngestionTrackingAudit> CreateIngestionTrackingAuditFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<IngestionTrackingAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.CreatedBy).Use(user)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.UpdatedBy).Use(user)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.IngestionTracking).IgnoreIt();

            return filler;
        }
    }
}
