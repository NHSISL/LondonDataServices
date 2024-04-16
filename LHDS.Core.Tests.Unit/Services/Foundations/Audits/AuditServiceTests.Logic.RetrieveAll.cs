using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.Audits;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public void ShouldReturnAudits()
        {
            // given
            IQueryable<Audit> randomAudits = CreateRandomAudits();
            IQueryable<Audit> storageAudits = randomAudits;
            IQueryable<Audit> expectedAudits = storageAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAudits())
                    .Returns(storageAudits);

            // when
            IQueryable<Audit> actualAudits =
                this.auditService.RetrieveAllAudits();

            // then
            actualAudits.Should().BeEquivalentTo(expectedAudits);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAudits(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}