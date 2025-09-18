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
        public async Task ShouldAddAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            Audit randomAudit =
                CreateRandomAudit(randomDateTimeOffset, randomEntraUserId);

            Audit inputAudit = randomAudit;
            Audit storageAudit = inputAudit;
            Audit expectedAudit = storageAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAuditAsync(inputAudit))
                    .ReturnsAsync(storageAudit);

            // when
            Audit actualAudit = await this.auditService
                .AddAuditAsync(inputAudit);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAuditAsync(inputAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}