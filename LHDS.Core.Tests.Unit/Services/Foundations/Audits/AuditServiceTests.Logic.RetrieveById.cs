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
        public async Task ShouldRetrieveAuditByIdAsync()
        {
            // given
            Audit randomAudit = CreateRandomAudit();
            Audit inputAudit = randomAudit;
            Audit storageAudit = randomAudit;
            Audit expectedAudit = storageAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAuditByIdAsync(inputAudit.Id))
                    .ReturnsAsync(storageAudit);

            // when
            Audit actualAudit =
                await this.auditService.RetrieveAuditByIdAsync(inputAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAuditByIdAsync(inputAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}