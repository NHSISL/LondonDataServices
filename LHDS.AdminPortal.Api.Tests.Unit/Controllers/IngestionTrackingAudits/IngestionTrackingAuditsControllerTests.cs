// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests : RESTFulController
    {
        private readonly Mock<IIngestionTrackingAuditService> ingestionTrackingAuditServiceMock;
        private readonly IngestionTrackingAuditsController ingestionTrackingAuditsController;

        public IngestionTrackingAuditsControllerTests()
        {
            this.ingestionTrackingAuditServiceMock = new Mock<IIngestionTrackingAuditService>();

            this.ingestionTrackingAuditsController = new IngestionTrackingAuditsController(
                this.ingestionTrackingAuditServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<IngestionTrackingAudit> CreateRandomIngestionTrackingAudits() =>
            CreateIngestionTrackingAuditFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static IngestionTrackingAudit CreateRandomIngestionTrackingAudit() =>
            CreateIngestionTrackingAuditFiller().Create();

        private static Filler<IngestionTrackingAudit> CreateIngestionTrackingAuditFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTrackingAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(accessAudit => accessAudit.CreatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.UpdatedBy).Use(user);

            return filler;
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new IngestionTrackingAuditDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new IngestionTrackingAuditServiceException(
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
                new IngestionTrackingAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new IngestionTrackingAuditDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}

