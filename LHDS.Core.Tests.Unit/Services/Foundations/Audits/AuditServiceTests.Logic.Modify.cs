using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.Audits;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public async Task ShouldModifyAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Audit randomAudit = CreateRandomModifyAudit(randomDateTimeOffset);
            Audit inputAudit = randomAudit;
            Audit storageAudit = inputAudit.DeepClone();
            storageAudit.UpdatedDate = randomAudit.CreatedDate;
            Audit updatedAudit = inputAudit;
            Audit expectedAudit = updatedAudit.DeepClone();
            Guid auditId = inputAudit.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAuditAsync(inputAudit))
                    .ReturnsAsync(updatedAudit);

            // when
            Audit actualAudit =
                await this.auditService.ModifyAuditAsync(inputAudit);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAuditAsync(inputAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}