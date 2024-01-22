// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldAddAddressExtractionAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit(randomDateTimeOffset);
            AddressExtractionAudit inputAddressExtractionAudit = randomAddressExtractionAudit;
            AddressExtractionAudit storageAddressExtractionAudit = inputAddressExtractionAudit;
            AddressExtractionAudit expectedAddressExtractionAudit = storageAddressExtractionAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAddressExtractionAuditAsync(inputAddressExtractionAudit))
                    .ReturnsAsync(storageAddressExtractionAudit);

            // when
            AddressExtractionAudit actualAddressExtractionAudit = await this.addressExtractionAuditService
                .AddAddressExtractionAuditAsync(inputAddressExtractionAudit);

            // then
            actualAddressExtractionAudit.Should().BeEquivalentTo(expectedAddressExtractionAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(inputAddressExtractionAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}