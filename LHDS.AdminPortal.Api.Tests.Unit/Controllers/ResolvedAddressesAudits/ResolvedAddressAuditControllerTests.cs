// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Services.Foundations.ResolvedAddressAudits;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using RESTFulSense.Controllers;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditControllerTests : RESTFulController
    {
        private readonly Mock<IResolvedAddressAuditService> resolvedAddressAuditServiceMock;
        private readonly ResolvedAddressesAuditController resolvedAddressAuditsController;

        public ResolvedAddressAuditControllerTests()
        {
            this.resolvedAddressAuditServiceMock = new Mock<IResolvedAddressAuditService>();

            this.resolvedAddressAuditsController = new ResolvedAddressesAuditController(
                this.resolvedAddressAuditServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<ResolvedAddressAudit> CreateRandomResolvedAddressAudits() =>
            CreateResolvedAddressAuditFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static ResolvedAddressAudit CreateRandomResolvedAddressAudit() =>
            CreateResolvedAddressAuditFiller().Create();

        private static Filler<ResolvedAddressAudit> CreateResolvedAddressAuditFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddressAudit>();

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
                new ResolvedAddressAuditDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new ResolvedAddressAuditServiceException(
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
                new ResolvedAddressAuditValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new ResolvedAddressAuditDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
