using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldModifyResolvedAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomModifyResolvedAddress(randomDateTimeOffset);
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress.DeepClone();
            storageResolvedAddress.UpdatedDate = randomResolvedAddress.CreatedDate;
            ResolvedAddress updatedResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = updatedResolvedAddress.DeepClone();
            Guid resolvedAddressId = inputResolvedAddress.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(updatedResolvedAddress);

            // when
            ResolvedAddress actualResolvedAddress =
                await this.resolvedAddressService.ModifyResolvedAddressAsync(inputResolvedAddress);

            // then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}