// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldModifyPdsAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            PdsAudit randomPdsAudit =
                CreateRandomModifyPdsAudit(randomDateTimeOffset, randomUserId);

            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit storagePdsAudit = inputPdsAudit.DeepClone();
            storagePdsAudit.UpdatedDate = randomPdsAudit.CreatedDate;
            PdsAudit updatedPdsAudit = inputPdsAudit;
            PdsAudit expectedPdsAudit = updatedPdsAudit.DeepClone();
            Guid pdsAuditId = inputPdsAudit.Id;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputPdsAudit))
                    .ReturnsAsync(inputPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(pdsAuditId))
                    .ReturnsAsync(storagePdsAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePdsAuditAsync(inputPdsAudit))
                    .ReturnsAsync(updatedPdsAudit);

            // when
            PdsAudit actualPdsAudit =
                await this.pdsAuditService.ModifyPdsAuditAsync(inputPdsAudit);

            // then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(inputPdsAudit.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(inputPdsAudit),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}