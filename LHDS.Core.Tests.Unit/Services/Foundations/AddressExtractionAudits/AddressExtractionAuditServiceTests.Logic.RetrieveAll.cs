// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public void ShouldReturnAddressExtractionAudits()
        {
            // given
            IQueryable<AddressExtractionAudit> randomAddressExtractionAudits = CreateRandomAddressExtractionAudits();
            IQueryable<AddressExtractionAudit> storageAddressExtractionAudits = randomAddressExtractionAudits;
            IQueryable<AddressExtractionAudit> expectedAddressExtractionAudits = storageAddressExtractionAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressExtractionAudits())
                    .Returns(storageAddressExtractionAudits);

            // when
            IQueryable<AddressExtractionAudit> actualAddressExtractionAudits =
                this.addressExtractionAuditService.RetrieveAllAddressExtractionAudits();

            // then
            actualAddressExtractionAudits.Should().BeEquivalentTo(expectedAddressExtractionAudits);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressExtractionAudits(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}