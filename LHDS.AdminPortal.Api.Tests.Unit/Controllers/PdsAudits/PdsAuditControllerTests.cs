// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudits
{
    public partial class PdsAuditControllerTests
    {
        private readonly Mock<IPdsAuditService> pdsAuditServiceMock;
        private readonly PdsAuditsController pdsAuditsController;

        public PdsAuditControllerTests()
        {
            this.pdsAuditServiceMock = new Mock<IPdsAuditService>();
            this.pdsAuditsController = new PdsAuditsController(this.pdsAuditServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<PdsAudit> CreateRandomPdsAudits() =>
            CreatePdsAuditFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static PdsAudit CreateRandomPdsAudit() =>
            CreatePdsAuditFiller().Create();

        private static Filler<PdsAudit> CreatePdsAuditFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<PdsAudit>();

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
                new PdsAuditDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new PdsAuditServiceException(
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
                new PdsAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new PdsAuditDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
