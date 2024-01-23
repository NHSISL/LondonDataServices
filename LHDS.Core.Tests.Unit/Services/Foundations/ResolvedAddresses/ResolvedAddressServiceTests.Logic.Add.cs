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
        public async Task ShouldAddResolvedAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(storageResolvedAddress);

            // when
            ResolvedAddress actualResolvedAddress = await this.resolvedAddressService
                .AddResolvedAddressAsync(inputResolvedAddress);

            // then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}