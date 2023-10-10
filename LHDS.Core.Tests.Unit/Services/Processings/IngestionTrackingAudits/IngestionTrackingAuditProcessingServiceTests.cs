// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        private readonly Mock<IAuditService> ingestionTrackingAuditServiceMock = new Mock<IAuditService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IIngestionTrackingAuditProcessingService ingestionTrackingAuditProcessingService;

        public IngestionTrackingAuditProcessingServiceTests()
        {
            ingestionTrackingAuditProcessingService = new IngestionTrackingAuditProcessingService(
                ingestionTrackingAuditService: ingestionTrackingAuditServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AuditValidationException(
                    message: "Audit validation errors occurred, please try again.", innerException),

                new AuditDependencyValidationException(
                    message: "Audit dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AuditDependencyException(
                    message: "Audit dependency validation errors occurred, please try again.", innerException),

                new AuditServiceException(
                    message : "Audit service error occurred, contact support.", innerException)
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

        private static Audit CreateRandomIngestionTrackingAudit() =>
            CreateIngestionTrackingAuditFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static IQueryable<Audit> CreateRandomIngestionTrackingAudits()
        {
            return CreateIngestionTrackingAuditFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<Audit> CreateIngestionTrackingAuditFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<Audit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);

            return filler;
        }
    }
}
