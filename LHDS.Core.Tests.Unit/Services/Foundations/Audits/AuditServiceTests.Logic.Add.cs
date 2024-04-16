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
        public async Task ShouldAddAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Audit randomAudit = CreateRandomAudit(randomDateTimeOffset);
            Audit inputAudit = randomAudit;
            Audit storageAudit = inputAudit;
            Audit expectedAudit = storageAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAuditAsync(inputAudit))
                    .ReturnsAsync(storageAudit);

            // when
            Audit actualAudit = await this.auditService
                .AddAuditAsync(inputAudit);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAuditAsync(inputAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}