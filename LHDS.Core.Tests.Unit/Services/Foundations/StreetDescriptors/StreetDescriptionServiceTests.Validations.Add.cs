// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Models.Foundations.StreetDescriptors;
using LHDS.Core.Services.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class StreetDescriptorServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfStreetDescriptorIsNullAndLogItAsync()
        {
            // given
            StreetDescriptor nullStreetDescriptor = null;

            var nullStreetDescriptorException =
                new NullStreetDescriptorException(message: "Street Descriptor is null.");

            var expectedStreetDescriptorValidationException =
                new StreetDescriptorValidationException(
                    message: "Street Descriptor validation errors occurred, please try again.",
                    innerException: nullStreetDescriptorException);

            // when
            ValueTask<StreetDescriptor> addStreetDescriptorTask =
                this.streetDescriptorService.AddAddressAsync(nullAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(addAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}