// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldEnsureCreatedAuditPropertiesIsSameAsStorageAsync()
        {
            // given
            ResolvedAddress inputResolvedAddress = CreateRandomResolvedAddress(GetRandomDateTimeOffset());
            ResolvedAddress maybeResolvedAddress = CreateRandomResolvedAddress(GetRandomDateTimeOffset());
            ResolvedAddress expectedResolvedAddress = inputResolvedAddress.DeepClone();
            expectedResolvedAddress.CreatedDate = maybeResolvedAddress.CreatedDate;
            expectedResolvedAddress.CreatedBy = maybeResolvedAddress.CreatedBy;

            var resolvedAddressServiceMock = new Mock<ResolvedAddressService>(
                storageBrokerMock.Object,
                identifierBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object,
                auditBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            ResolvedAddress actualResolvedAddress =
                await resolvedAddressServiceMock.Object.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputResolvedAddress, maybeResolvedAddress);

            // then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            resolvedAddressServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    inputResolvedAddress, maybeResolvedAddress),
                        Times.Once());

            resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}