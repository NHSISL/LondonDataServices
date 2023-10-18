using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressLoadingAudits
{
    public partial class AddressLoadingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddAddressLoadingAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit storageAddressLoadingAudit = inputAddressLoadingAudit;
            AddressLoadingAudit expectedAddressLoadingAudit = storageAddressLoadingAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.addressLoadingAuditServiceMock.Setup(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit))
                    .ReturnsAsync(storageAddressLoadingAudit);

            // when
            AddressLoadingAudit actualAddressLoadingAudit = await this.addressLoadingAuditProcessingService
                .AddAddressLoadingAuditAsync(inputAddressLoadingAudit);

            // then
            actualAddressLoadingAudit.Should().BeEquivalentTo(expectedAddressLoadingAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.addressLoadingAuditServiceMock.Verify(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.addressLoadingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}