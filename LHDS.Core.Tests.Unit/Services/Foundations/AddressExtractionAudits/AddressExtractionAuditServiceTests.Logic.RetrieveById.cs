// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAddressExtractionAuditByIdAsync()
        {
            // given
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            AddressExtractionAudit inputAddressExtractionAudit = randomAddressExtractionAudit;
            AddressExtractionAudit storageAddressExtractionAudit = randomAddressExtractionAudit;
            AddressExtractionAudit expectedAddressExtractionAudit = storageAddressExtractionAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(inputAddressExtractionAudit.Id))
                    .ReturnsAsync(storageAddressExtractionAudit);

            // when
            AddressExtractionAudit actualAddressExtractionAudit =
                await this.addressExtractionAuditService.RetrieveAddressExtractionAuditByIdAsync(inputAddressExtractionAudit.Id);

            // then
            actualAddressExtractionAudit.Should().BeEquivalentTo(expectedAddressExtractionAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(inputAddressExtractionAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}