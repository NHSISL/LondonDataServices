// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Audits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public async Task ShouldAddAuditLogAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomIdentifier = Guid.NewGuid();
            string randomAuditType = GetRandomString();
            string randomAuditTitle = GetRandomString();
            string randomMesssage = GetRandomString();
            string randomFileName = GetRandomString();
            string randomLogLevel = GetRandomString();

            Audit randomAudit = new Audit
            {
                Id = randomIdentifier,
                AuditType = randomAuditType,
                Title = randomAuditTitle,
                Message = randomMesssage,
                CorrelationId = randomIdentifier,
                FileName = randomFileName,
                LogLevel = randomLogLevel,
                CreatedBy = "System",
                CreatedDate = randomDateTimeOffset,
                UpdatedBy = "System",
                UpdatedDate = randomDateTimeOffset,
            };

            Audit inputAudit = randomAudit;
            Audit storageAudit = inputAudit;
            Audit expectedAudit = inputAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomIdentifier);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAuditAsync(It.Is(SameAuditAs(inputAudit))))
                    .ReturnsAsync(storageAudit);

            // when
            Audit actualAudit = await this.auditService
                .AddAuditAsync(
                    auditType: randomAuditType,
                    title: randomAuditTitle,
                    message: randomMesssage,
                    fileName: randomFileName,
                    correlationId: randomIdentifier,
                    logLevel: randomLogLevel);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAuditAsync(It.Is(SameAuditAs(inputAudit))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}